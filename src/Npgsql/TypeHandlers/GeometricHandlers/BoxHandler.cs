﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Npgsql.BackendMessages;
using NpgsqlTypes;

namespace Npgsql.TypeHandlers.GeometricHandlers
{
    /// <summary>
    /// Type handler for the PostgreSQL geometric box type.
    /// </summary>
    /// <remarks>
    /// http://www.postgresql.org/docs/current/static/datatype-geometric.html
    /// </remarks>
    [TypeMapping("box", NpgsqlDbType.Box, typeof(NpgsqlBox))]
    internal class BoxHandler : TypeHandler<NpgsqlBox>,
        ISimpleTypeReader<NpgsqlBox>, ISimpleTypeWriter,
        ISimpleTypeReader<string>
    {
        public NpgsqlBox Read(NpgsqlBuffer buf, int len, FieldDescription fieldDescription)
        {
            return new NpgsqlBox(
                new NpgsqlPoint(buf.ReadDouble(), buf.ReadDouble()),
                new NpgsqlPoint(buf.ReadDouble(), buf.ReadDouble())
            );
        }

        string ISimpleTypeReader<string>.Read(NpgsqlBuffer buf, int len, FieldDescription fieldDescription)
        {
            return Read(buf, len, fieldDescription).ToString();
        }

        public int ValidateAndGetLength(object value)
        {
            return 32;
        }

        public void Write(object value, NpgsqlBuffer buf)
        {
            var s = value as string;
            var v = s != null ? NpgsqlBox.Parse(s) : (NpgsqlBox)value;
            buf.WriteDouble(v.Right);
            buf.WriteDouble(v.Top);
            buf.WriteDouble(v.Left);
            buf.WriteDouble(v.Bottom);
        }
    }
}
