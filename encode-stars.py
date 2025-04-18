import struct
import numpy as np

def convert_to_temperature(spectral_code):
    if spectral_code == b'WN':
        return 100000
    elif spectral_code == b'WC':
        return 65000
    else:
        ranges = [ 0, 1, 2, 3, 4, 5, 6, 7 ]
        temperatures = [ 40000.0, 30000.0, 10000.0, 7500.0, 6000.0, 5200.0, 3700.0, 2400.0 ]
        if spectral_code[0] == b'C'[0]:
            temperatures[6] = 3500.0
        elif spectral_code[0] == b'N'[0]:
            temperatures[6] = 3000.0
        code = { b'O'[0]: 0, b'B'[0]: 1, b'A'[0]: 2, b'F'[0]: 3, b'G'[0]: 4, b'K'[0]: 5, b'M'[0]: 6, b'S'[0]: 6, b'C'[0]: 6, b'N'[0]: 6 }
        fraction = (spectral_code[1] - b'0'[0]) * 0.1 + 0.05
        return np.interp(code[spectral_code[0]] + fraction, ranges, temperatures)

def parse_stars(filename):
    stars = []
    with open(filename, "rb") as file:
        header = struct.unpack("<IIiIIII", file.read(28))
        sequence_number = header[0]
        first_star = header[1]
        number_of_stars = abs(header[2])
        J2000 = header[2] < 0
        star_id_in_catalog = header[3] == 1
        proper_motion = header[4] == 1
        number_of_magnitudes = header[5]
        record_length = header[6]
        for star in range(0, number_of_stars):
            record = struct.unpack("<fdd2shff", file.read(record_length))
            number = record[0] + sequence_number
            right_ascension = record[1]
            declination = record[2]
            spectral_type = record[3]
            visual_magnitude = record[4] / 100
            relative_brightness = 10**(-visual_magnitude / 5)
            right_ascension_proper_motion = record[5]
            declination_proper_motion = record[6]
            valid_record = not spectral_type == b'  ' and not spectral_type == b'pe'
            if valid_record:
                temperature = convert_to_temperature(spectral_type)
                stars.append((temperature, right_ascension, declination, relative_brightness))
    return stars
            
stars = parse_stars("BSC5")
stars = sorted(stars, key=lambda star: star[0], reverse=True)
boilerplate = """
using System.Collections.Generic;

public class BrightStarCatalogueData
{{
    public static IEnumerable<BrightStarCatalogueEntry> Entries()
    {{
{}
    }}
}}
"""
data = []
for star in stars:
    line = f'        yield return new BrightStarCatalogueEntry({star[0]}, {star[1]}f, {star[2]}f, {star[3]}f);'
    data.append(line)
with open("BrightStarCatalogueData.cs", "w") as file:
    file.write(boilerplate.format('\n'.join(data)))
