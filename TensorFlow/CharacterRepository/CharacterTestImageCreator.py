__author__ = 'dennis'
# -*- coding: utf-8 -*-


from PIL import Image, ImageDraw, ImageFont

#				"Verdana",
# "Arial",
#				"Arial Black",
#				"Courier New",
#				"Georgia"
# Declare font files and encodings
FONT1 = ('Arial.ttf', 'unicode')
FONT2 = ('Arial_Black.ttf', 'unicode')
FONT3 = ('Arial_Bold.ttf', 'unicode')
FONT4 = ('Arial_Bold_Italic.ttf', 'unicode')
FONT5 = ('Arial_Italic.ttf', 'unicode')
FONT6 = ('Times_New_Roman.ttf', 'unicode')
FONT7 = ('Times_New_Roman_Bold.ttf', 'unicode')
FONT8 = ('Times_New_Roman_Bold_Italic.ttf', 'unicode')
FONT9 = ('Times_New_Roman_Italic.ttf', 'unicode')
FONT10 = ('Courier_New.ttf', 'unicode')

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
                (FONT10, tuple(DIGITS))]

TEST_GLYPHS = [(FONT5, tuple(DIGITS)),
               (FONT9, tuple(DIGITS))]


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
    draw.text((0, 0), text=unicode_char, font=font)
    return img


def save_glyphs(glyphs, mode="train"):
    # Save an image for each glyph we want to draw
    for (font_file, font_encoding), unicode_chars in glyphs:
        for char in unicode_chars:
            img = draw_glyph(font_file, font_encoding, char)
            index = unicode_chars.index(char)
            filename = 'data/%s_%s-%d.jpg' % (mode, str.partition(font_file, '.')[0], index)
            img.save(filename)
            with open('%s_images.txt' % mode, 'a') as f: f.write('%s %i\r\n' % (filename, index))

save_glyphs(TRAIN_GLYPHS)
save_glyphs(TEST_GLYPHS, "test")
