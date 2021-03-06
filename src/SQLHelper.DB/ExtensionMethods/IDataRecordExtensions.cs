﻿/*
Copyright 2016 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using BigBook;
using System;
using System.Data;

namespace SQLHelper.ExtensionMethods
{
    /// <summary>
    /// Extension methods for IDataRecord objects
    /// </summary>
    public static class IDataRecordExtensions
    {
        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <param name="reader">Reader object</param>
        /// <param name="id">Parameter name</param>
        /// <param name="defaultValue">Default value for the parameter</param>
        /// <returns>
        /// if the parameter exists (and isn't null or empty), it returns the parameter's value.
        /// Otherwise the default value is returned.
        /// </returns>
        public static DataType GetParameter<DataType>(this IDataRecord reader, string id, DataType defaultValue = default(DataType))
        {
            if (reader == null)
                return defaultValue;
            for (int x = 0; x < reader.FieldCount; ++x)
            {
                if (reader.GetName(x) == id)
                    return reader.GetParameter(x, defaultValue);
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns a parameter's value
        /// </summary>
        /// <param name="reader">Reader object</param>
        /// <param name="position">Position in the reader row</param>
        /// <param name="defaultValue">Default value for the parameter</param>
        /// <returns>
        /// if the parameter exists (and isn't null or empty), it returns the parameter's value.
        /// Otherwise the default value is returned.
        /// </returns>
        public static DataType GetParameter<DataType>(this IDataRecord reader, int position, DataType defaultValue = default(DataType))
        {
            if (reader == null)
                return defaultValue;
            object Value = reader[position];
            return (Value == null || DBNull.Value == Value) ? defaultValue : Value.To(defaultValue);
        }
    }
}