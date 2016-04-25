/*
$Id: 72fcc267dadd9c98525b6b73f644fa7e97e82f85 $

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
namespace com.itextpdf.barcodes.qrcode
{
	/// <author>satorux@google.com (Satoru Takabayashi) - creator</author>
	/// <author>dswitkin@google.com (Daniel Switkin) - ported from C++</author>
	internal sealed class MatrixUtil
	{
		private MatrixUtil()
		{
		}

		private static readonly int[][] POSITION_DETECTION_PATTERN = new int[][] { new int
			[] { 1, 1, 1, 1, 1, 1, 1 }, new int[] { 1, 0, 0, 0, 0, 0, 1 }, new int[] { 1, 0, 
			1, 1, 1, 0, 1 }, new int[] { 1, 0, 1, 1, 1, 0, 1 }, new int[] { 1, 0, 1, 1, 1, 0
			, 1 }, new int[] { 1, 0, 0, 0, 0, 0, 1 }, new int[] { 1, 1, 1, 1, 1, 1, 1 } };

		private static readonly int[][] HORIZONTAL_SEPARATION_PATTERN = new int[][] { new 
			int[] { 0, 0, 0, 0, 0, 0, 0, 0 } };

		private static readonly int[][] VERTICAL_SEPARATION_PATTERN = new int[][] { new int
			[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new int[] { 0 }, new 
			int[] { 0 }, new int[] { 0 } };

		private static readonly int[][] POSITION_ADJUSTMENT_PATTERN = new int[][] { new int
			[] { 1, 1, 1, 1, 1 }, new int[] { 1, 0, 0, 0, 1 }, new int[] { 1, 0, 1, 0, 1 }, 
			new int[] { 1, 0, 0, 0, 1 }, new int[] { 1, 1, 1, 1, 1 } };

		private static readonly int[][] POSITION_ADJUSTMENT_PATTERN_COORDINATE_TABLE = new 
			int[][] { new int[] { -1, -1, -1, -1, -1, -1, -1 }, new int[] { 6, 18, -1, -1, -
			1, -1, -1 }, new int[] { 6, 22, -1, -1, -1, -1, -1 }, new int[] { 6, 26, -1, -1, 
			-1, -1, -1 }, new int[] { 6, 30, -1, -1, -1, -1, -1 }, new int[] { 6, 34, -1, -1
			, -1, -1, -1 }, new int[] { 6, 22, 38, -1, -1, -1, -1 }, new int[] { 6, 24, 42, 
			-1, -1, -1, -1 }, new int[] { 6, 26, 46, -1, -1, -1, -1 }, new int[] { 6, 28, 50
			, -1, -1, -1, -1 }, new int[] { 6, 30, 54, -1, -1, -1, -1 }, new int[] { 6, 32, 
			58, -1, -1, -1, -1 }, new int[] { 6, 34, 62, -1, -1, -1, -1 }, new int[] { 6, 26
			, 46, 66, -1, -1, -1 }, new int[] { 6, 26, 48, 70, -1, -1, -1 }, new int[] { 6, 
			26, 50, 74, -1, -1, -1 }, new int[] { 6, 30, 54, 78, -1, -1, -1 }, new int[] { 6
			, 30, 56, 82, -1, -1, -1 }, new int[] { 6, 30, 58, 86, -1, -1, -1 }, new int[] { 
			6, 34, 62, 90, -1, -1, -1 }, new int[] { 6, 28, 50, 72, 94, -1, -1 }, new int[] 
			{ 6, 26, 50, 74, 98, -1, -1 }, new int[] { 6, 30, 54, 78, 102, -1, -1 }, new int
			[] { 6, 28, 54, 80, 106, -1, -1 }, new int[] { 6, 32, 58, 84, 110, -1, -1 }, new 
			int[] { 6, 30, 58, 86, 114, -1, -1 }, new int[] { 6, 34, 62, 90, 118, -1, -1 }, 
			new int[] { 6, 26, 50, 74, 98, 122, -1 }, new int[] { 6, 30, 54, 78, 102, 126, -
			1 }, new int[] { 6, 26, 52, 78, 104, 130, -1 }, new int[] { 6, 30, 56, 82, 108, 
			134, -1 }, new int[] { 6, 34, 60, 86, 112, 138, -1 }, new int[] { 6, 30, 58, 86, 
			114, 142, -1 }, new int[] { 6, 34, 62, 90, 118, 146, -1 }, new int[] { 6, 30, 54
			, 78, 102, 126, 150 }, new int[] { 6, 24, 50, 76, 102, 128, 154 }, new int[] { 6
			, 28, 54, 80, 106, 132, 158 }, new int[] { 6, 32, 58, 84, 110, 136, 162 }, new int
			[] { 6, 26, 54, 82, 110, 138, 166 }, new int[] { 6, 30, 58, 86, 114, 142, 170 } };

		private static readonly int[][] TYPE_INFO_COORDINATES = new int[][] { new int[] { 
			8, 0 }, new int[] { 8, 1 }, new int[] { 8, 2 }, new int[] { 8, 3 }, new int[] { 
			8, 4 }, new int[] { 8, 5 }, new int[] { 8, 7 }, new int[] { 8, 8 }, new int[] { 
			7, 8 }, new int[] { 5, 8 }, new int[] { 4, 8 }, new int[] { 3, 8 }, new int[] { 
			2, 8 }, new int[] { 1, 8 }, new int[] { 0, 8 } };

		private const int VERSION_INFO_POLY = 0x1f25;

		private const int TYPE_INFO_POLY = 0x537;

		private const int TYPE_INFO_MASK_PATTERN = 0x5412;

		// From Appendix E. Table 1, JIS0510X:2004 (p 71). The table was double-checked by komatsu.
		// Version 1
		// Version 2
		// Version 3
		// Version 4
		// Version 5
		// Version 6
		// Version 7
		// Version 8
		// Version 9
		// Version 10
		// Version 11
		// Version 12
		// Version 13
		// Version 14
		// Version 15
		// Version 16
		// Version 17
		// Version 18
		// Version 19
		// Version 20
		// Version 21
		// Version 22
		// Version 23
		// Version 24
		// Version 25
		// Version 26
		// Version 27
		// Version 28
		// Version 29
		// Version 30
		// Version 31
		// Version 32
		// Version 33
		// Version 34
		// Version 35
		// Version 36
		// Version 37
		// Version 38
		// Version 39
		// Version 40
		// Type info cells at the left top corner.
		// From Appendix D in JISX0510:2004 (p. 67)
		// 1 1111 0010 0101
		// From Appendix C in JISX0510:2004 (p.65).
		// Set all cells to -1.  -1 means that the cell is empty (not set yet).
		//
		// JAVAPORT: We shouldn't need to do this at all. The code should be rewritten to begin encoding
		// with the ByteMatrix initialized all to zero.
		public static void ClearMatrix(ByteMatrix matrix)
		{
			matrix.Clear(unchecked((byte)-1));
		}

		// Build 2D matrix of QR Code from "dataBits" with "ecLevel", "version" and "getMaskPattern". On
		// success, store the result in "matrix" and return true.
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		public static void BuildMatrix(BitVector dataBits, ErrorCorrectionLevel ecLevel, 
			int version, int maskPattern, ByteMatrix matrix)
		{
			ClearMatrix(matrix);
			EmbedBasicPatterns(version, matrix);
			// Type information appear with any version.
			EmbedTypeInfo(ecLevel, maskPattern, matrix);
			// Version info appear if version >= 7.
			MaybeEmbedVersionInfo(version, matrix);
			// Data should be embedded at end.
			EmbedDataBits(dataBits, maskPattern, matrix);
		}

		// Embed basic patterns. On success, modify the matrix and return true.
		// The basic patterns are:
		// - Position detection patterns
		// - Timing patterns
		// - Dark dot at the left bottom corner
		// - Position adjustment patterns, if need be
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		public static void EmbedBasicPatterns(int version, ByteMatrix matrix)
		{
			// Let's get started with embedding big squares at corners.
			EmbedPositionDetectionPatternsAndSeparators(matrix);
			// Then, embed the dark dot at the left bottom corner.
			EmbedDarkDotAtLeftBottomCorner(matrix);
			// Position adjustment patterns appear if version >= 2.
			MaybeEmbedPositionAdjustmentPatterns(version, matrix);
			// Timing patterns should be embedded after position adj. patterns.
			EmbedTimingPatterns(matrix);
		}

		// Embed type information. On success, modify the matrix.
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		public static void EmbedTypeInfo(ErrorCorrectionLevel ecLevel, int maskPattern, ByteMatrix
			 matrix)
		{
			BitVector typeInfoBits = new BitVector();
			MakeTypeInfoBits(ecLevel, maskPattern, typeInfoBits);
			for (int i = 0; i < typeInfoBits.Size(); ++i)
			{
				// Place bits in LSB to MSB order.  LSB (least significant bit) is the last value in
				// "typeInfoBits".
				int bit = typeInfoBits.At(typeInfoBits.Size() - 1 - i);
				// Type info bits at the left top corner. See 8.9 of JISX0510:2004 (p.46).
				int x1 = TYPE_INFO_COORDINATES[i][0];
				int y1 = TYPE_INFO_COORDINATES[i][1];
				matrix.Set(x1, y1, bit);
				if (i < 8)
				{
					// Right top corner.
					int x2 = matrix.GetWidth() - i - 1;
					int y2 = 8;
					matrix.Set(x2, y2, bit);
				}
				else
				{
					// Left bottom corner.
					int x2 = 8;
					int y2 = matrix.GetHeight() - 7 + (i - 8);
					matrix.Set(x2, y2, bit);
				}
			}
		}

		// Embed version information if need be. On success, modify the matrix and return true.
		// See 8.10 of JISX0510:2004 (p.47) for how to embed version information.
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		public static void MaybeEmbedVersionInfo(int version, ByteMatrix matrix)
		{
			if (version < 7)
			{
				// Version info is necessary if version >= 7.
				return;
			}
			// Don't need version info.
			BitVector versionInfoBits = new BitVector();
			MakeVersionInfoBits(version, versionInfoBits);
			int bitIndex = 6 * 3 - 1;
			// It will decrease from 17 to 0.
			for (int i = 0; i < 6; ++i)
			{
				for (int j = 0; j < 3; ++j)
				{
					// Place bits in LSB (least significant bit) to MSB order.
					int bit = versionInfoBits.At(bitIndex);
					bitIndex--;
					// Left bottom corner.
					matrix.Set(i, matrix.GetHeight() - 11 + j, bit);
					// Right bottom corner.
					matrix.Set(matrix.GetHeight() - 11 + j, i, bit);
				}
			}
		}

		// Embed "dataBits" using "getMaskPattern". On success, modify the matrix and return true.
		// For debugging purposes, it skips masking process if "getMaskPattern" is -1.
		// See 8.7 of JISX0510:2004 (p.38) for how to embed data bits.
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		public static void EmbedDataBits(BitVector dataBits, int maskPattern, ByteMatrix 
			matrix)
		{
			int bitIndex = 0;
			int direction = -1;
			// Start from the right bottom cell.
			int x = matrix.GetWidth() - 1;
			int y = matrix.GetHeight() - 1;
			while (x > 0)
			{
				// Skip the vertical timing pattern.
				if (x == 6)
				{
					x -= 1;
				}
				while (y >= 0 && y < matrix.GetHeight())
				{
					for (int i = 0; i < 2; ++i)
					{
						int xx = x - i;
						// Skip the cell if it's not empty.
						if (!IsEmpty(matrix.Get(xx, y)))
						{
							continue;
						}
						int bit;
						if (bitIndex < dataBits.Size())
						{
							bit = dataBits.At(bitIndex);
							++bitIndex;
						}
						else
						{
							// Padding bit. If there is no bit left, we'll fill the left cells with 0, as described
							// in 8.4.9 of JISX0510:2004 (p. 24).
							bit = 0;
						}
						// Skip masking if mask_pattern is -1.
						if (maskPattern != -1)
						{
							if (MaskUtil.GetDataMaskBit(maskPattern, xx, y))
							{
								bit ^= 0x1;
							}
						}
						matrix.Set(xx, y, bit);
					}
					y += direction;
				}
				direction = -direction;
				// Reverse the direction.
				y += direction;
				x -= 2;
			}
			// Move to the left.
			// All bits should be consumed.
			if (bitIndex != dataBits.Size())
			{
				throw new WriterException("Not all bits consumed: " + bitIndex + '/' + dataBits.Size
					());
			}
		}

		// Return the position of the most significant bit set (to one) in the "value". The most
		// significant bit is position 32. If there is no bit set, return 0. Examples:
		// - findMSBSet(0) => 0
		// - findMSBSet(1) => 1
		// - findMSBSet(255) => 8
		public static int FindMSBSet(int value)
		{
			int numDigits = 0;
			while (value != 0)
			{
				value = (int)(((uint)value) >> 1);
				++numDigits;
			}
			return numDigits;
		}

		// Calculate BCH (Bose-Chaudhuri-Hocquenghem) code for "value" using polynomial "poly". The BCH
		// code is used for encoding type information and version information.
		// Example: Calculation of version information of 7.
		// f(x) is created from 7.
		//   - 7 = 000111 in 6 bits
		//   - f(x) = x^2 + x^2 + x^1
		// g(x) is given by the standard (p. 67)
		//   - g(x) = x^12 + x^11 + x^10 + x^9 + x^8 + x^5 + x^2 + 1
		// Multiply f(x) by x^(18 - 6)
		//   - f'(x) = f(x) * x^(18 - 6)
		//   - f'(x) = x^14 + x^13 + x^12
		// Calculate the remainder of f'(x) / g(x)
		//         x^2
		//         __________________________________________________
		//   g(x) )x^14 + x^13 + x^12
		//         x^14 + x^13 + x^12 + x^11 + x^10 + x^7 + x^4 + x^2
		//         --------------------------------------------------
		//                              x^11 + x^10 + x^7 + x^4 + x^2
		//
		// The remainder is x^11 + x^10 + x^7 + x^4 + x^2
		// Encode it in binary: 110010010100
		// The return value is 0xc94 (1100 1001 0100)
		//
		// Since all coefficients in the polynomials are 1 or 0, we can do the calculation by bit
		// operations. We don't care if cofficients are positive or negative.
		public static int CalculateBCHCode(int value, int poly)
		{
			// If poly is "1 1111 0010 0101" (version info poly), msbSetInPoly is 13. We'll subtract 1
			// from 13 to make it 12.
			int msbSetInPoly = FindMSBSet(poly);
			value <<= msbSetInPoly - 1;
			// Do the division business using exclusive-or operations.
			while (FindMSBSet(value) >= msbSetInPoly)
			{
				value ^= poly << (FindMSBSet(value) - msbSetInPoly);
			}
			// Now the "value" is the remainder (i.e. the BCH code)
			return value;
		}

		// Make bit vector of type information. On success, store the result in "bits" and return true.
		// Encode error correction level and mask pattern. See 8.9 of
		// JISX0510:2004 (p.45) for details.
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		public static void MakeTypeInfoBits(ErrorCorrectionLevel ecLevel, int maskPattern
			, BitVector bits)
		{
			if (!QRCode.IsValidMaskPattern(maskPattern))
			{
				throw new WriterException("Invalid mask pattern");
			}
			int typeInfo = (ecLevel.GetBits() << 3) | maskPattern;
			bits.AppendBits(typeInfo, 5);
			int bchCode = CalculateBCHCode(typeInfo, TYPE_INFO_POLY);
			bits.AppendBits(bchCode, 10);
			BitVector maskBits = new BitVector();
			maskBits.AppendBits(TYPE_INFO_MASK_PATTERN, 15);
			bits.Xor(maskBits);
			if (bits.Size() != 15)
			{
				// Just in case.
				throw new WriterException("should not happen but we got: " + bits.Size());
			}
		}

		// Make bit vector of version information. On success, store the result in "bits" and return true.
		// See 8.10 of JISX0510:2004 (p.45) for details.
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		public static void MakeVersionInfoBits(int version, BitVector bits)
		{
			bits.AppendBits(version, 6);
			int bchCode = CalculateBCHCode(version, VERSION_INFO_POLY);
			bits.AppendBits(bchCode, 12);
			if (bits.Size() != 18)
			{
				// Just in case.
				throw new WriterException("should not happen but we got: " + bits.Size());
			}
		}

		// Check if "value" is empty.
		private static bool IsEmpty(int value)
		{
			return value == -1;
		}

		// Check if "value" is valid.
		private static bool IsValidValue(int value)
		{
			return (value == -1 || value == 0 || value == 1);
		}

		// Empty.
		// Light (white).
		// Dark (black).
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		private static void EmbedTimingPatterns(ByteMatrix matrix)
		{
			// -8 is for skipping position detection patterns (size 7), and two horizontal/vertical
			// separation patterns (size 1). Thus, 8 = 7 + 1.
			for (int i = 8; i < matrix.GetWidth() - 8; ++i)
			{
				int bit = (i + 1) % 2;
				// Horizontal line.
				if (!IsValidValue(matrix.Get(i, 6)))
				{
					throw new WriterException();
				}
				if (IsEmpty(matrix.Get(i, 6)))
				{
					matrix.Set(i, 6, bit);
				}
				// Vertical line.
				if (!IsValidValue(matrix.Get(6, i)))
				{
					throw new WriterException();
				}
				if (IsEmpty(matrix.Get(6, i)))
				{
					matrix.Set(6, i, bit);
				}
			}
		}

		// Embed the lonely dark dot at left bottom corner. JISX0510:2004 (p.46)
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		private static void EmbedDarkDotAtLeftBottomCorner(ByteMatrix matrix)
		{
			if (matrix.Get(8, matrix.GetHeight() - 8) == 0)
			{
				throw new WriterException();
			}
			matrix.Set(8, matrix.GetHeight() - 8, 1);
		}

		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		private static void EmbedHorizontalSeparationPattern(int xStart, int yStart, ByteMatrix
			 matrix)
		{
			// We know the width and height.
			if (HORIZONTAL_SEPARATION_PATTERN[0].Length != 8 || HORIZONTAL_SEPARATION_PATTERN
				.Length != 1)
			{
				throw new WriterException("Bad horizontal separation pattern");
			}
			for (int x = 0; x < 8; ++x)
			{
				if (!IsEmpty(matrix.Get(xStart + x, yStart)))
				{
					throw new WriterException();
				}
				matrix.Set(xStart + x, yStart, HORIZONTAL_SEPARATION_PATTERN[0][x]);
			}
		}

		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		private static void EmbedVerticalSeparationPattern(int xStart, int yStart, ByteMatrix
			 matrix)
		{
			// We know the width and height.
			if (VERTICAL_SEPARATION_PATTERN[0].Length != 1 || VERTICAL_SEPARATION_PATTERN.Length
				 != 7)
			{
				throw new WriterException("Bad vertical separation pattern");
			}
			for (int y = 0; y < 7; ++y)
			{
				if (!IsEmpty(matrix.Get(xStart, yStart + y)))
				{
					throw new WriterException();
				}
				matrix.Set(xStart, yStart + y, VERTICAL_SEPARATION_PATTERN[y][0]);
			}
		}

		// Note that we cannot unify the function with embedPositionDetectionPattern() despite they are
		// almost identical, since we cannot write a function that takes 2D arrays in different sizes in
		// C/C++. We should live with the fact.
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		private static void EmbedPositionAdjustmentPattern(int xStart, int yStart, ByteMatrix
			 matrix)
		{
			// We know the width and height.
			if (POSITION_ADJUSTMENT_PATTERN[0].Length != 5 || POSITION_ADJUSTMENT_PATTERN.Length
				 != 5)
			{
				throw new WriterException("Bad position adjustment");
			}
			for (int y = 0; y < 5; ++y)
			{
				for (int x = 0; x < 5; ++x)
				{
					if (!IsEmpty(matrix.Get(xStart + x, yStart + y)))
					{
						throw new WriterException();
					}
					matrix.Set(xStart + x, yStart + y, POSITION_ADJUSTMENT_PATTERN[y][x]);
				}
			}
		}

		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		private static void EmbedPositionDetectionPattern(int xStart, int yStart, ByteMatrix
			 matrix)
		{
			// We know the width and height.
			if (POSITION_DETECTION_PATTERN[0].Length != 7 || POSITION_DETECTION_PATTERN.Length
				 != 7)
			{
				throw new WriterException("Bad position detection pattern");
			}
			for (int y = 0; y < 7; ++y)
			{
				for (int x = 0; x < 7; ++x)
				{
					if (!IsEmpty(matrix.Get(xStart + x, yStart + y)))
					{
						throw new WriterException();
					}
					matrix.Set(xStart + x, yStart + y, POSITION_DETECTION_PATTERN[y][x]);
				}
			}
		}

		// Embed position detection patterns and surrounding vertical/horizontal separators.
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		private static void EmbedPositionDetectionPatternsAndSeparators(ByteMatrix matrix
			)
		{
			// Embed three big squares at corners.
			int pdpWidth = POSITION_DETECTION_PATTERN[0].Length;
			// Left top corner.
			EmbedPositionDetectionPattern(0, 0, matrix);
			// Right top corner.
			EmbedPositionDetectionPattern(matrix.GetWidth() - pdpWidth, 0, matrix);
			// Left bottom corner.
			EmbedPositionDetectionPattern(0, matrix.GetWidth() - pdpWidth, matrix);
			// Embed horizontal separation patterns around the squares.
			int hspWidth = HORIZONTAL_SEPARATION_PATTERN[0].Length;
			// Left top corner.
			EmbedHorizontalSeparationPattern(0, hspWidth - 1, matrix);
			// Right top corner.
			EmbedHorizontalSeparationPattern(matrix.GetWidth() - hspWidth, hspWidth - 1, matrix
				);
			// Left bottom corner.
			EmbedHorizontalSeparationPattern(0, matrix.GetWidth() - hspWidth, matrix);
			// Embed vertical separation patterns around the squares.
			int vspSize = VERTICAL_SEPARATION_PATTERN.Length;
			// Left top corner.
			EmbedVerticalSeparationPattern(vspSize, 0, matrix);
			// Right top corner.
			EmbedVerticalSeparationPattern(matrix.GetHeight() - vspSize - 1, 0, matrix);
			// Left bottom corner.
			EmbedVerticalSeparationPattern(vspSize, matrix.GetHeight() - vspSize, matrix);
		}

		// Embed position adjustment patterns if need be.
		/// <exception cref="com.itextpdf.barcodes.qrcode.WriterException"/>
		private static void MaybeEmbedPositionAdjustmentPatterns(int version, ByteMatrix 
			matrix)
		{
			if (version < 2)
			{
				// The patterns appear if version >= 2
				return;
			}
			int index = version - 1;
			int[] coordinates = POSITION_ADJUSTMENT_PATTERN_COORDINATE_TABLE[index];
			int numCoordinates = POSITION_ADJUSTMENT_PATTERN_COORDINATE_TABLE[index].Length;
			for (int i = 0; i < numCoordinates; ++i)
			{
				for (int j = 0; j < numCoordinates; ++j)
				{
					int y = coordinates[i];
					int x = coordinates[j];
					if (x == -1 || y == -1)
					{
						continue;
					}
					// If the cell is unset, we embed the position adjustment pattern here.
					if (IsEmpty(matrix.Get(x, y)))
					{
						// -2 is necessary since the x/y coordinates point to the center of the pattern, not the
						// left top corner.
						EmbedPositionAdjustmentPattern(x - 2, y - 2, matrix);
					}
				}
			}
		}
	}
}
