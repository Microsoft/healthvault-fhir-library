﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// MIT License
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Linq;
using Hl7.Fhir.Model;
using Microsoft.HealthVault.Fhir.Constants;
using Microsoft.HealthVault.ItemTypes;

namespace Microsoft.HealthVault.Fhir.Transformers
{
    internal static class CodingToCodedValue
    {
        public static CodedValue ToCodedValue(this Coding coding)
        {
            var uri = new Uri(coding.System);
            var returnValue = new CodedValue
            {
                Value = coding.Code,
                Version = coding.Version,
            };

            var family = GetFamily(uri);
            if (!string.IsNullOrEmpty(family))
            {
                returnValue.Family = family;
            }

            var vocabName = GetVocabularyName(uri);
            if (!string.IsNullOrEmpty(vocabName))
            {
                returnValue.VocabularyName = vocabName;
            }

            return returnValue;
        }

        private static string GetFamily(Uri uri)
        {
            if (uri.ToString().ToLowerInvariant().Contains(HealthVaultVocabularies.BaseUri.ToLowerInvariant()))
            { 
                // Expected to cotain 6 if the family is specified in the URL
                if (uri.Segments.Length == 6)
                {
                    return uri.Segments[4].TrimEnd('/');
                }

                // By default if nothing is specified, then wc is assumed
                return "wc";
            }

            return null;
        }

        private static string GetVocabularyName(Uri uri)
        {
            if (uri.ToString().ToLowerInvariant().Contains(HealthVaultVocabularies.BaseUri.ToLowerInvariant()))
            {
                return uri.Segments.Last();
            }

            return null;
        }
    }
}
