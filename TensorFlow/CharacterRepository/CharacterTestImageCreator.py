__author__ = 'dennis'
# -*- coding: utf-8 -*-

import os
import random
from PIL import Image, ImageDraw, ImageFont

#				"Verdana",
# "Arial",
#				"Arial Black",
#				"Courier New",
#				"Georgia"
# Declare font files and encodings
#FONT1 = ('Arial.ttf', 'unicode')
#FONT2 = ('Arial_Black.ttf', 'unicode')
#FONT3 = ('Arial_Bold.ttf', 'unicode')
#FONT4 = ('Arial_Bold_Italic.ttf', 'unicode')
#FONT5 = ('Arial_Italic.ttf', 'unicode')
#FONT6 = ('Times_New_Roman.ttf', 'unicode')
#FONT7 = ('Times_New_Roman_Bold.ttf', 'unicode')
#FONT8 = ('Times_New_Roman_Bold_Italic.ttf', 'unicode')
#FONT9 = ('Times_New_Roman_Italic.ttf', 'unicode')
#FONT10 = ('Courier_New.ttf', 'unicode')
FONT1 = ('Aaargh.ttf', 'unicode')
FONT2 = ('Aller_BdIt.ttf', 'unicode')
FONT3 = ('Aller_Bd.ttf', 'unicode')
FONT4 = ('AllerDisplay.ttf', 'unicode')
FONT5 = ('Aller_It.ttf', 'unicode')
FONT6= ('Aller_LtIt.ttf', 'unicode')
FONT7 = ('Aller_Lt.ttf', 'unicode')
FONT8 = ('Aller_Rg.ttf', 'unicode')
FONT9 = ('Arial_Black.ttf', 'unicode')
FONT10 = ('Arial_Bold_Italic.ttf', 'unicode')
FONT11 = ('Arial_Bold.ttf', 'unicode')
FONT12 = ('Arial_Italic.ttf', 'unicode')
FONT13 = ('Arial.ttf', 'unicode')
FONT14 = ('CaviarDreams_BoldItalic.ttf', 'unicode')
FONT15 = ('Caviar_Dreams_Bold.ttf', 'unicode')
FONT16 = ('CaviarDreams_Italic.ttf', 'unicode')
FONT17 = ('CaviarDreams.ttf', 'unicode')
FONT18 = ('Courier_New.ttf', 'unicode')
FONT19 = ('Lato-BlackItalic.ttf', 'unicode')
FONT20 = ('Lato-Black.ttf', 'unicode')
FONT21 = ('Lato-BoldItalic.ttf', 'unicode')
FONT22 = ('Lato-Bold.ttf', 'unicode')
#FONT23 = ('Lato-HairlineItalic.ttf', 'unicode')
#FONT24 = ('Lato-Hairline.ttf', 'unicode')
FONT25 = ('Lato-HeavyItalic.ttf', 'unicode')
FONT26 = ('Lato-Heavy.ttf', 'unicode')
FONT27 = ('Lato-Italic.ttf', 'unicode')
FONT28 = ('Lato-LightItalic.ttf', 'unicode')
FONT29 = ('Lato-Light.ttf', 'unicode')
FONT30 = ('Lato-MediumItalic.ttf', 'unicode')
FONT31 = ('Lato-Medium.ttf', 'unicode')
FONT32 = ('Lato-Regular.ttf', 'unicode')
FONT33 = ('Lato-SemiboldItalic.ttf', 'unicode')
FONT34 = ('Lato-Semibold.ttf', 'unicode')
#FONT35 = ('Lato-ThinItalic.ttf', 'unicode')
#FONT36 = ('Lato-Thin.ttf', 'unicode')
FONT37 = ('OpenSans-BoldItalic.ttf', 'unicode')
FONT38 = ('OpenSans-Bold.ttf', 'unicode')
FONT39 = ('OpenSans-ExtraBoldItalic.ttf', 'unicode')
FONT40 = ('OpenSans-ExtraBold.ttf', 'unicode')
FONT41 = ('OpenSans-Italic.ttf', 'unicode')
FONT42 = ('OpenSans-LightItalic.ttf', 'unicode')
FONT43 = ('OpenSans-Light.ttf', 'unicode')
FONT44 = ('OpenSans-Regular.ttf', 'unicode')
FONT45 = ('OpenSans-SemiboldItalic.ttf', 'unicode')
FONT46 = ('OpenSans-Semibold.ttf', 'unicode')
FONT47 = ('PTC55F.ttf', 'unicode')
FONT48 = ('PTC75F.ttf', 'unicode')
FONT49 = ('PTN57F.ttf', 'unicode')
FONT50 = ('PTN77F.ttf', 'unicode')
FONT51 = ('PTS55F.ttf', 'unicode')
FONT52 = ('PTS56F.ttf', 'unicode')
FONT53 = ('PTS75F.ttf', 'unicode')
FONT54 = ('PTS76F.ttf', 'unicode')
FONT55 = ('Raleway-BlackItalic.ttf', 'unicode')
FONT56 = ('Raleway-Black.ttf', 'unicode')
FONT57 = ('Raleway-BoldItalic.ttf', 'unicode')
FONT58 = ('Raleway-Bold.ttf', 'unicode')
FONT59 = ('Raleway-ExtraBoldItalic.ttf', 'unicode')
FONT60 = ('Raleway-ExtraBold.ttf', 'unicode')
#FONT61 = ('Raleway-ExtraLightItalic.ttf', 'unicode')
#FONT62 = ('Raleway-ExtraLight.ttf', 'unicode')
FONT63 = ('Raleway-Italic.ttf', 'unicode')
FONT64 = ('Raleway-LightItalic.ttf', 'unicode')
FONT65 = ('Raleway-Light.ttf', 'unicode')
FONT66 = ('Raleway-MediumItalic.ttf', 'unicode')
FONT67 = ('Raleway-Medium.ttf', 'unicode')
FONT68 = ('Raleway-Regular.ttf', 'unicode')
FONT69 = ('Raleway-SemiBoldItalic.ttf', 'unicode')
FONT70 = ('Raleway-SemiBold.ttf', 'unicode')
#FONT71 = ('Raleway-ThinItalic.ttf', 'unicode')
#FONT72 = ('Raleway-Thin.ttf', 'unicode')
FONT73 = ('Roboto-BlackItalic.ttf', 'unicode')
FONT74 = ('Roboto-Black.ttf', 'unicode')
FONT75 = ('Roboto-BoldItalic.ttf', 'unicode')
FONT76 = ('Roboto-Bold.ttf', 'unicode')
FONT77 = ('RobotoCondensed-BoldItalic.ttf', 'unicode')
FONT78 = ('RobotoCondensed-Bold.ttf', 'unicode')
FONT79 = ('RobotoCondensed-Italic.ttf', 'unicode')
FONT80 = ('RobotoCondensed-LightItalic.ttf', 'unicode')
FONT81 = ('RobotoCondensed-Light.ttf', 'unicode')
FONT82 = ('RobotoCondensed-Regular.ttf', 'unicode')
FONT83 = ('Roboto-Italic.ttf', 'unicode')
FONT84 = ('Roboto-LightItalic.ttf', 'unicode')
FONT85 = ('Roboto-Light.ttf', 'unicode')
FONT86 = ('Roboto-MediumItalic.ttf', 'unicode')
FONT87 = ('Roboto-Medium.ttf', 'unicode')
FONT88 = ('Roboto-Regular.ttf', 'unicode')
#FONT89 = ('Roboto-ThinItalic.ttf', 'unicode')
#FONT90 = ('Roboto-Thin.ttf', 'unicode')
FONT91 = ('Sansation-BoldItalic.ttf', 'unicode')
FONT92 = ('Sansation-Bold.ttf', 'unicode')
FONT93 = ('Sansation-Italic.ttf', 'unicode')
FONT94 = ('Sansation-LightItalic.ttf', 'unicode')
FONT95 = ('Sansation-Light.ttf', 'unicode')
FONT96 = ('Sansation-Regular.ttf', 'unicode')
FONT97 = ('Times_New_Roman_Bold_Italic.ttf', 'unicode')
FONT98 = ('Times_New_Roman_Bold.ttf', 'unicode')
FONT99 = ('Times_New_Roman_Italic.ttf', 'unicode')
FONT100 = ('Times_New_Roman.ttf', 'unicode')
FONT101 = ('Walkway_Black.ttf', 'unicode')
FONT102 = ('Walkway_Bold.ttf', 'unicode')
FONT103 = ('Walkway_Oblique_Black.ttf', 'unicode')
FONT104 = ('Walkway_Oblique_Bold.ttf', 'unicode')
FONT105 = ('Walkway_Oblique_SemiBold.ttf', 'unicode')
FONT106 = ('Walkway_Oblique.ttf', 'unicode')
FONT107 = ('Walkway_Oblique_UltraBold.ttf', 'unicode')
FONT108 = ('Walkway_SemiBold.ttf', 'unicode')
FONT109 = ('Walkway_UltraBold.ttf', 'unicode')



CHARACTERS = [
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W',
    'X',
    'Y', 'Z',
    'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w',
    'x',
    'y', 'z']
DIGITS = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']
SPECIALCHARACTERS = [
    ',', '.', '-', '#', '+', 'ß', ';', ':', '_', '*', '~', '?', '=', ')', '(', '/', '&', '%', '$', '§', '"', '!', '@',
    '€']

# Declare a mapping from encodings used by str.encode() to encodings used by
# the FreeType library
ENCODING_MAP = {'unicode': 'unic',
                'big5': 'big5',
                'big5hkscs': 'big5',
                'shift-jis': 'sjis'}

# The glyphs we want to draw
# GLYPHS = [(FONT1, tuple(CHARACTERS + DIGITS + SPECIALCHARACTERS)),
#           (FONT2, tuple(CHARACTERS + DIGITS + SPECIALCHARACTERS)),
#           (FONT3, tuple(CHARACTERS + DIGITS + SPECIALCHARACTERS))]

TRAIN_GLYPHS = [(FONT1, tuple(DIGITS)),
                (FONT2, tuple(DIGITS)),
                (FONT3, tuple(DIGITS)),
                (FONT4, tuple(DIGITS)),
                (FONT6, tuple(DIGITS)),
                (FONT7, tuple(DIGITS)),
                (FONT8, tuple(DIGITS)),
                (FONT9, tuple(DIGITS)),
                (FONT10, tuple(DIGITS)),
                (FONT11, tuple(DIGITS)),
                (FONT12, tuple(DIGITS)),
                (FONT13, tuple(DIGITS)),
                (FONT14, tuple(DIGITS)),
                (FONT16, tuple(DIGITS)),
                (FONT17, tuple(DIGITS)),
                (FONT18, tuple(DIGITS)),
                (FONT19, tuple(DIGITS)),
                (FONT20, tuple(DIGITS)),
                (FONT21, tuple(DIGITS)),
                (FONT22, tuple(DIGITS)),
              #  (FONT23, tuple(DIGITS)),
              #  (FONT24, tuple(DIGITS)),
                (FONT25, tuple(DIGITS)),
                (FONT26, tuple(DIGITS)),
                (FONT27, tuple(DIGITS)),
                (FONT28, tuple(DIGITS)),
                (FONT29, tuple(DIGITS)),
                (FONT30, tuple(DIGITS)),
                (FONT31, tuple(DIGITS)),
                (FONT32, tuple(DIGITS)),
                (FONT33, tuple(DIGITS)),
                (FONT34, tuple(DIGITS)),
              #  (FONT35, tuple(DIGITS)),
              #  (FONT36, tuple(DIGITS)),
                (FONT37, tuple(DIGITS)),
                (FONT38, tuple(DIGITS)),
                (FONT39, tuple(DIGITS)),
                (FONT40, tuple(DIGITS)),
                (FONT41, tuple(DIGITS)),
                (FONT42, tuple(DIGITS)),
                (FONT43, tuple(DIGITS)),
                (FONT44, tuple(DIGITS)),
                (FONT45, tuple(DIGITS)),
                (FONT46, tuple(DIGITS)),
                (FONT47, tuple(DIGITS)),
                (FONT48, tuple(DIGITS)),
                (FONT49, tuple(DIGITS)),
                (FONT50, tuple(DIGITS)),
                (FONT51, tuple(DIGITS)),
               (FONT52, tuple(DIGITS)),
               (FONT53, tuple(DIGITS)),
               (FONT54, tuple(DIGITS)),
               (FONT55, tuple(DIGITS)),
               (FONT56, tuple(DIGITS)),
               (FONT57, tuple(DIGITS)),
               (FONT58, tuple(DIGITS)),
               (FONT59, tuple(DIGITS)),
               (FONT60, tuple(DIGITS)),
            #   (FONT61, tuple(DIGITS)),
            #   (FONT62, tuple(DIGITS)),
               (FONT63, tuple(DIGITS)),
               (FONT64, tuple(DIGITS)),
               (FONT65, tuple(DIGITS)),
               (FONT66, tuple(DIGITS)),
               (FONT67, tuple(DIGITS)),
               (FONT68, tuple(DIGITS)),
               (FONT69, tuple(DIGITS)),
               (FONT70, tuple(DIGITS)),
            #   (FONT71, tuple(DIGITS)),
            #   (FONT72, tuple(DIGITS)),
               (FONT73, tuple(DIGITS)),
               (FONT74, tuple(DIGITS)),
               (FONT75, tuple(DIGITS)),
               (FONT76, tuple(DIGITS)),
               (FONT77, tuple(DIGITS)),
               (FONT78, tuple(DIGITS)),
               (FONT79, tuple(DIGITS)),
               (FONT80, tuple(DIGITS)),
               (FONT81, tuple(DIGITS)),
               (FONT82, tuple(DIGITS)),
               (FONT83, tuple(DIGITS)),
               (FONT84, tuple(DIGITS)),
               (FONT85, tuple(DIGITS)),
               (FONT86, tuple(DIGITS)),
               (FONT87, tuple(DIGITS)),
               (FONT88, tuple(DIGITS)),
           #    (FONT89, tuple(DIGITS)),
           #    (FONT90, tuple(DIGITS)),
               (FONT91, tuple(DIGITS)),
               (FONT92, tuple(DIGITS)),
               (FONT93, tuple(DIGITS)),
               (FONT94, tuple(DIGITS)),
               (FONT95, tuple(DIGITS)),
               (FONT96, tuple(DIGITS)),
               (FONT97, tuple(DIGITS)),
               (FONT98, tuple(DIGITS)),
               (FONT99, tuple(DIGITS))]

TEST_GLYPHS = [(FONT100, tuple(DIGITS)),
               (FONT101, tuple(DIGITS)),
               (FONT102, tuple(DIGITS)),
               (FONT103, tuple(DIGITS)),
               (FONT104, tuple(DIGITS)),
               (FONT105, tuple(DIGITS)),
               (FONT106, tuple(DIGITS)),
               (FONT107, tuple(DIGITS)),
               (FONT108, tuple(DIGITS)),
               (FONT109, tuple(DIGITS))
               ]


# Returns PIL Image object
def draw_glyph(font_file, font_encoding, unicode_char, glyph_size=28):
    # Translate unicode string if necessary
    if font_encoding != 'unicode':
        mb_string = unicode_char.encode(font_encoding)
        try:
            # Try using Python 2.x's unichr
            unicode_char = unichr(ord(mb_string[0]) << 8 | ord(mb_string[1]))
        except NameError:
            # Use Python 3.x-compatible code
            unicode_char = chr(mb_string[0] << 8 | mb_string[1])

    # Load font using mapped encoding
    font = ImageFont.truetype(font_file, glyph_size, encoding=ENCODING_MAP[font_encoding])
    # font = ImageFont.load(font_file)

    # Now draw the glyph
    img = Image.new('L', (glyph_size, glyph_size), 'white')
    draw = ImageDraw.Draw(img)
    draw.text((5, -2), text=unicode_char, font=font)
    return img


def save_glyphs(glyphs, mode='train'):
    # Save an image for each glyph we want to draw
    for (font_file, font_encoding), unicode_chars in glyphs:
        for char in unicode_chars:
            img = draw_glyph('fonts/%s' % font_file, font_encoding, char)
            index = unicode_chars.index(char)
            filename = 'data/%s_%s-%d.jpg' % (mode, str.partition(font_file, '.')[0], index)
            img.save(filename)
            with open('%s_images.txt' % mode, 'a') as f: f.write('%s %i\r\n' % (filename, index))

def import_glyps_from_Chars74K(rootFolder, mode='train'):
    inputs = []
    for root, dirs, _ in os.walk(rootFolder):
            for subdir in dirs:
                index = int(subdir[(len(subdir)-3):]) - 1
                for _, _, files in os.walk(root + '/' + subdir):
                    for filename in files[:min(50, len(files))]:
                        relativeFilename = (os.path.join(root, subdir, filename))
                        inputs.append((relativeFilename, index))
    random.shuffle(inputs)
    with open('%s_images.txt' % mode, 'a') as f:
        for entry in inputs:
            f.write('%s %i\r\n' % (entry[0] , entry[1]))

if os.path.exists('train_images.txt'):
    os.remove('train_images.txt')
if os.path.exists('test_images.txt'):
    os.remove('test_images.txt')

#save_glyphs(TRAIN_GLYPHS)
#save_glyphs(TEST_GLYPHS, "test")

import_glyps_from_Chars74K('data/Fnt')
import_glyps_from_Chars74K('data/Fnt', 'test')
