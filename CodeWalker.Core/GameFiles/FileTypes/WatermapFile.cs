using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using EXP = System.ComponentModel.ExpandableObjectConverter;
using TC = System.ComponentModel.TypeConverterAttribute;

namespace CodeWalker.GameFiles
{
    [TC(typeof(EXP))]
    public class WatermapFile : GameFile, PackedFile
    {
        public byte[] RawFileData { get; set; }

        public uint Magic { get; set; } = 0x574D4150; //'WMAP'
        public uint Version { get; set; } = 100;
        public uint DataLength { get; set; } //59360 - data length
        public float CornerX { get; set; } //-4050.0f  - topleft X
        public float CornerY { get; set; } //8400.0f   - topleft Y
        public float TileX { get; set; } //50.0f  - tile size X
        public float TileY { get; set; } //50.0f  - tile size Y (step negative?)
        public ushort Width { get; set; } //183  - image Width
        public ushort Height { get; set; } //249  - image Height
        public uint WatermapIndsCount { get; set; } //10668
        public uint WatermapRefsCount { get; set; } //11796
        public ushort RiverVecsCount { get; set; } //99
        public ushort RiverCount { get; set; } //13
        public ushort LakeVecsCount { get; set; } //28
        public ushort LakeCount { get; set; } //15
        public ushort PoolCount { get; set; } //314
        public ushort ColoursOffset { get; set; } //13316 
        public byte[] Unks1 { get; set; }//2,2,16,48,16,48,32,0   ..?

        public CompHeader[] CompHeaders { get; set; }
        public short[] CompWatermapInds { get; set; }//indices into CompWatermapRefs
        public WaterItemRef[] CompWatermapRefs { get; set; }//contains multibit, type, index1, [index2](optional)
        public byte[] Zeros1 { get; set; }//x12
        public Vector4[] RiverVecs { get; set; }
        public WaterFlow[] Rivers { get; set; }
        public Vector4[] LakeVecs { get; set; }
        public WaterFlow[] Lakes { get; set; }
        public WaterPool[] Pools { get; set; }
        public Color[] Colours { get; set; }//x342
        public uint ColourCount { get; set; }//342 (RiverCount + LakeCount + PoolCount)


        private static readonly byte[] Empty8 = new byte[8];
        private static readonly byte[] Empty12 = new byte[12];


        public short[] GridWatermapInds { get; set; } //expanded from CompWatermapInds.
        public WaterItemRef[][] GridWatermapRefs { get; set; } //expanded from CompWatermapHeaders. ends up max 7 items


        public WatermapFile() : base(null, GameFileType.Watermap)
        {
        }
        public WatermapFile(RpfFileEntry entry) : base(entry, GameFileType.Watermap)
        {
            RpfFileEntry = entry;
        }

        public void Load(byte[] data, RpfFileEntry entry)
        {
            RawFileData = data;
            if (entry != null)
            {
                RpfFileEntry = entry;
                Name = entry.Name;
            }

            using (MemoryStream ms = new MemoryStream(data))
            {
                DataReader r = new DataReader(ms, Endianess.BigEndian);

                Read(r);
            }
        }

        public byte[] Save()
        {
            MemoryStream s = new MemoryStream();
            DataWriter w = new DataWriter(s, Endianess.BigEndian);

            Write(w);

            var buf = new byte[s.Length];
            s.Position = 0;
            s.Read(buf, 0, buf.Length);
            return buf;
        }

        private static void WriteArray<T>(DataWriter w, T[] items, Action<DataWriter, T> writeOne)
        {
            if (items == null || items.Length == 0) return;
            for (int i = 0; i < items.Length; i++)
                writeOne(w, items[i]);
        }

        private void Read(DataReader r)
        {
            Magic = r.ReadUInt32();//'WMAP'
            Version = r.ReadUInt32();//100 - version?
            DataLength = r.ReadUInt32();//59360 - data length (excluding last flags array!)
            CornerX = r.ReadSingle();//-4050.0f  - min XY?
            CornerY = r.ReadSingle();//8400.0f   - max XY?
            TileX = r.ReadSingle();//50.0f  - tile size X
            TileY = r.ReadSingle();//50.0f  - tile size Y
            Width = r.ReadUInt16();//183  - image Width
            Height = r.ReadUInt16();//249  - image Height
            WatermapIndsCount = r.ReadUInt32();//10668
            WatermapRefsCount = r.ReadUInt32();//11796
            RiverVecsCount = r.ReadUInt16();//99
            RiverCount = r.ReadUInt16();//13
            LakeVecsCount = r.ReadUInt16();//28
            LakeCount = r.ReadUInt16();//15
            PoolCount = r.ReadUInt16();//314
            ColoursOffset = r.ReadUInt16();//13316    
            Unks1 = r.ReadBytes(8);//2,2,16,48,16,48,32,0      flags..?


            var shortslen = (int)((WatermapIndsCount + WatermapRefsCount) * 2) + (Height * 4);//offset from here to Zeros1
            var padcount = (16 - (shortslen % 16)) % 16;//12 .. is this right? all are zeroes.
            var strucslen = ((RiverVecsCount + LakeVecsCount) * 16) + ((RiverCount + LakeCount) * 48) + (PoolCount * 32);
            var datalen = shortslen + padcount + strucslen; //DataLength calculation
            var extoffs = padcount + strucslen - 60 - 60;//ExtraFlagsOffset calculation


            CompHeaders = new CompHeader[Height];//249 - image height
            for (int i = 0; i < Height; i++) CompHeaders[i].Read(r);

            CompWatermapInds = new short[WatermapIndsCount];//10668
            for (int i = 0; i < WatermapIndsCount; i++) CompWatermapInds[i] = r.ReadInt16();

            CompWatermapRefs = new WaterItemRef[WatermapRefsCount];//11796
            for (int i = 0; i < WatermapRefsCount; i++) CompWatermapRefs[i] = new WaterItemRef(r.ReadUInt16());

            Zeros1 = r.ReadBytes(padcount);//align to 16 bytes (position:45984)
            
            RiverVecs = new Vector4[RiverVecsCount];//99
            for (int i = 0; i < RiverVecsCount; i++) RiverVecs[i] = r.ReadVector4();
            
            Rivers = new WaterFlow[RiverCount];//13
            for (int i = 0; i < RiverCount; i++) Rivers[i] = new WaterFlow(WaterItemType.River, r, RiverVecs);
            
            LakeVecs = new Vector4[LakeVecsCount];//28
            for (int i = 0; i < LakeVecsCount; i++) LakeVecs[i] = r.ReadVector4();
            
            Lakes = new WaterFlow[LakeCount];//15
            for (int i = 0; i < LakeCount; i++) Lakes[i] = new WaterFlow(WaterItemType.Lake, r, LakeVecs);
            
            Pools = new WaterPool[PoolCount];//314
            for (int i = 0; i < PoolCount; i++) Pools[i] = new WaterPool(r);

            ColourCount = (uint)(RiverCount + LakeCount + PoolCount); //342
            Colours = new Color[ColourCount]; //342
            for (int i = 0; i < 342; i++) Colours[i] = Color.FromAbgr(r.ReadUInt32());


            var flagoff = 0; //assign extra colours out of the main array
            for (int i = 0; i < Rivers.Length; i++)
            {
                var river = Rivers[i];
                river.Colour = Colours[flagoff++];
            }
            for (int i = 0; i < Lakes.Length; i++)
            {
                var lake = Lakes[i];
                lake.Colour = Colours[flagoff++];
            }
            for (int i = 0; i < Pools.Length; i++)
            {
                var pool = Pools[i];
                pool.Colour = Colours[flagoff++];
            }



            for (int i = 0; i < CompWatermapRefs.Length; i++) //assign items to CompWatermapRefs
            {
                var ir = CompWatermapRefs[i];
                switch (ir.Type)
                {
                    case WaterItemType.River: CompWatermapRefs[i].Item = Rivers[ir.ItemIndex]; break;
                    case WaterItemType.Lake: CompWatermapRefs[i].Item = Lakes[ir.ItemIndex]; break;
                    case WaterItemType.Pool: CompWatermapRefs[i].Item = Pools[ir.ItemIndex]; break;
                }
            }



            //decompress main data into grid form
            GridWatermapInds = new short[Width * Height];
            GridWatermapRefs = new WaterItemRef[Width * Height][];
            var reflist = new List<WaterItemRef>();
            for (int y = 0; y < Height; y++)
            {
                var ch = CompHeaders[y];
                for (int i = 0; i < ch.Count; i++)
                {
                    var x = ch.Start + i;
                    var n = CompWatermapInds[ch.Offset + i];
                    var o = y * Width + x;
                    
                    reflist.Clear();
                    WaterItemRef[] refarr = null;
                    if (n >= 0)
                    {
                        var h = CompWatermapRefs[n];
                        reflist.Add(h);
                        var cn = n;
                        while (h.EndOfList == false)
                        {
                            cn++;
                            h = CompWatermapRefs[cn];
                            reflist.Add(h);
                        }

                        refarr = reflist.ToArray();
                    }

                    GridWatermapInds[o] = n;
                    GridWatermapRefs[o] = refarr;
                }
            }

            var rem = r.Length - r.Position;//60788
            if (rem != 0)
            { }
        }
        private void Write(DataWriter w)
        {
            CompHeaders = new CompHeader[Height];
            CompWatermapInds = Array.Empty<short>();
            CompWatermapRefs = Array.Empty<WaterItemRef>();
            Zeros1 = Empty12;
            RiverVecs = Array.Empty<Vector4>();
            Rivers = Array.Empty<WaterFlow>();
            LakeVecs = Array.Empty<Vector4>();
            Lakes = Array.Empty<WaterFlow>();
            Pools = Array.Empty<WaterPool>();
            Colours = Array.Empty<Color>();

            WatermapIndsCount = (uint)CompWatermapInds.Length;
            WatermapRefsCount = (uint)CompWatermapRefs.Length;
            RiverVecsCount = (ushort)RiverVecs.Length;
            RiverCount = (ushort)Rivers.Length;
            LakeVecsCount = (ushort)LakeVecs.Length;
            LakeCount = (ushort)Lakes.Length;
            PoolCount = (ushort)Pools.Length;
            ColourCount = (uint)Colours.Length;

            long headerStart = w.Position;

            w.Write(Magic);
            w.Write(Version);
            long dataLengthPos = w.Position;
            w.Write(0u);
            w.Write(CornerX);
            w.Write(CornerY);
            w.Write(TileX);
            w.Write(TileY);
            w.Write(Width);
            w.Write(Height);
            w.Write(WatermapIndsCount);
            w.Write(WatermapRefsCount);
            w.Write(RiverVecsCount);
            w.Write(RiverCount);
            w.Write(LakeVecsCount);
            w.Write(LakeCount);
            w.Write(PoolCount);

            long coloursOffsetPos = w.Position;
            w.Write((ushort)0);

            if (Unks1 == null || Unks1.Length == 0)
            {
                w.Write(Empty8);
            }
            else
            {
                if (Unks1.Length == 8)
                {
                    w.Write(Unks1);
                }
                else
                {
                    w.Write(Unks1);
                    w.Write(new byte[8 - Unks1.Length]);
                }
            }

            long dataStart = w.Position;

            if (CompHeaders == null || CompHeaders.Length != Height)
            {
                CompHeaders = new CompHeader[Height];
            }
            for (int i = 0; i < CompHeaders.Length; i++)
            {
                (CompHeaders[i] = new CompHeader()).Write(w);
            }

            for (int i = 0; i < CompWatermapInds.Length; i++)
                w.Write(CompWatermapInds[i]);

            WriteArray(w, CompWatermapRefs, (ww, r) => r.Write(ww));
            w.Write(Zeros1);

            for (int i = 0; i < RiverVecs.Length; i++) w.Write(RiverVecs[i]);

            WriteArray(w, Rivers, (ww, rf) => rf.Write(ww));
            for (int i = 0; i < LakeVecs.Length; i++) w.Write(LakeVecs[i]);
            WriteArray(w, Lakes, (ww, lf) => lf.Write(ww));
            WriteArray(w, Pools, (ww, p) => p.Write(ww));

            ushort coloursOffsetVal = (ushort)Math.Max(0, w.Position - dataStart);
            for (int i = 0; i < Colours.Length; i++) w.Write((int)Colours[i]);

            uint dataLen = (uint)Math.Max(0, w.Position - dataStart);

            long endPos = w.Position;

            w.Position = dataLengthPos;
            w.Write(dataLen);

            w.Position = coloursOffsetPos;
            w.Write(coloursOffsetVal);

            w.Position = endPos;
        }


        public void WriteXml(StringBuilder sb, int indent)
        {
            WatermapXml.ValueTag(sb, indent, "Magic", $"0x{Magic:X8}");
            WatermapXml.ValueTag(sb, indent, "Version", Version.ToString());
            WatermapXml.ValueTag(sb, indent, "CornerX", FloatUtil.ToString(CornerX));
            WatermapXml.ValueTag(sb, indent, "CornerY", FloatUtil.ToString(CornerY));
            WatermapXml.ValueTag(sb, indent, "TileX", FloatUtil.ToString(TileX));
            WatermapXml.ValueTag(sb, indent, "TileY", FloatUtil.ToString(TileY));
            WatermapXml.ValueTag(sb, indent, "Width", Width.ToString());
            WatermapXml.ValueTag(sb, indent, "Height", Height.ToString());
            WatermapXml.ValueTag(sb, indent, "ColoursOffset", ColoursOffset.ToString());

            WatermapXml.ValueTag(sb, indent, "WatermapIndsCount", WatermapIndsCount.ToString());
            WatermapXml.ValueTag(sb, indent, "WatermapRefsCount", WatermapRefsCount.ToString());
            WatermapXml.ValueTag(sb, indent, "RiverVecsCount", RiverVecsCount.ToString());
            WatermapXml.ValueTag(sb, indent, "RiverCount", RiverCount.ToString());
            WatermapXml.ValueTag(sb, indent, "LakeVecsCount", LakeVecsCount.ToString());
            WatermapXml.ValueTag(sb, indent, "LakeCount", LakeCount.ToString());
            WatermapXml.ValueTag(sb, indent, "PoolCount", PoolCount.ToString());
            WatermapXml.ValueTag(sb, indent, "ColourCount", ColourCount.ToString());

            if (Unks1 != null && Unks1.Length > 0)
                WatermapXml.WriteRawArray(sb, Unks1, indent, "Unks1", "", WatermapXml.FormatHexByte, Unks1.Length);

            if (CompHeaders != null && CompHeaders.Length > 0)
            {
                WatermapXml.OpenTag(sb, indent, "CompHeaders");
                foreach (var h in CompHeaders)
                    WatermapXml.SelfClosingTag(sb, indent + 1, $"Header Start=\"{h.Start}\" Count=\"{h.Count}\" Offset=\"{h.Offset}\"");
                WatermapXml.CloseTag(sb, indent, "CompHeaders");
            }

            if (CompWatermapInds != null && CompWatermapInds.Length > 0)
                WatermapXml.WriteRawArray(sb, CompWatermapInds.Select(v => (ushort)v).ToArray(), indent, "CompWatermapInds", "", WatermapXml.FormatHexUInt16, Width);

            if (CompWatermapRefs != null && CompWatermapRefs.Length > 0)
                WatermapXml.WriteRawArray(sb, CompWatermapRefs.Select(v => v.RawValue).ToArray(), indent, "CompWatermapRefs", "", WatermapXml.FormatHexUInt16, Width);

            WatermapXml.OpenTag(sb, indent, "Rivers");
            for (int i = 0; i < Rivers?.Length; i++)
                Rivers[i].WriteXml(sb, indent + 1, $"River{i}");
            WatermapXml.CloseTag(sb, indent, "Rivers");

            WatermapXml.OpenTag(sb, indent, "Lakes");
            for (int i = 0; i < Lakes?.Length; i++)
                Lakes[i].WriteXml(sb, indent + 1, $"Lake{i}");
            WatermapXml.CloseTag(sb, indent, "Lakes");

            WatermapXml.OpenTag(sb, indent, "Pools");
            for (int i = 0; i < Pools?.Length; i++)
                Pools[i].WriteXml(sb, indent + 1, $"Pool{i}");
            WatermapXml.CloseTag(sb, indent, "Pools");

            if (RiverVecs != null && RiverVecs.Length > 0)
                WatermapXml.WriteVectorArray(sb, RiverVecs, indent, "RiverVecs");
            if (LakeVecs != null && LakeVecs.Length > 0)
                WatermapXml.WriteVectorArray(sb, LakeVecs, indent, "LakeVecs");

            if (Colours != null && Colours.Length > 0)
            {
                var colorInts = Colours.Select(c => (uint)c.ToRgba()).ToArray();
                WatermapXml.WriteRawArray(sb, colorInts, indent, "Colours", "", WatermapXml.FormatHexUInt32, 8);
            }
        }

        public void ReadXml(XmlNode node)
        {
            Magic = Xml.GetChildUIntAttribute(node, "Magic");
            Version = Xml.GetChildUIntAttribute(node, "Version");
            CornerX = Xml.GetChildFloatAttribute(node, "CornerX");
            CornerY = Xml.GetChildFloatAttribute(node, "CornerY");
            TileX = Xml.GetChildFloatAttribute(node, "TileX");
            TileY = Xml.GetChildFloatAttribute(node, "TileY");
            Width = (ushort)Xml.GetChildUIntAttribute(node, "Width");
            Height = (ushort)Xml.GetChildUIntAttribute(node, "Height");
            ColoursOffset = (ushort)Xml.GetChildUIntAttribute(node, "ColoursOffset");

            WatermapIndsCount = Xml.GetChildUIntAttribute(node, "WatermapIndsCount");
            WatermapRefsCount = Xml.GetChildUIntAttribute(node, "WatermapRefsCount");
            RiverVecsCount = (ushort)Xml.GetChildUIntAttribute(node, "RiverVecsCount");
            RiverCount = (ushort)Xml.GetChildUIntAttribute(node, "RiverCount");
            LakeVecsCount = (ushort)Xml.GetChildUIntAttribute(node, "LakeVecsCount");
            LakeCount = (ushort)Xml.GetChildUIntAttribute(node, "LakeCount");
            PoolCount = (ushort)Xml.GetChildUIntAttribute(node, "PoolCount");
            ColourCount = Xml.GetChildUIntAttribute(node, "ColourCount");

            Unks1 = Xml.GetChildRawByteArray(node, "Unks1");

            var hdrNode = node.SelectSingleNode("CompHeaders");
            if (hdrNode != null)
            {
                var hdrList = new List<CompHeader>();
                foreach (XmlNode hn in hdrNode.ChildNodes)
                {
                    if (hn.Name != "Header") continue;
                    var ch = new CompHeader
                    {
                        Start = (byte)Xml.GetUIntAttribute(hn, "Start"),
                        Count = (byte)Xml.GetUIntAttribute(hn, "Count"),
                        Offset = (ushort)Xml.GetUIntAttribute(hn, "Offset")
                    };
                    hdrList.Add(ch);
                }
                CompHeaders = hdrList.ToArray();
            }

            CompWatermapInds = Xml.GetChildRawShortArray(node, "CompWatermapInds");
            var refsRaw = Xml.GetChildRawUShortArray(node, "CompWatermapRefs");
            if (refsRaw != null)
                CompWatermapRefs = refsRaw.Select(v => new WaterItemRef(v)).ToArray();

            var riversNode = node.SelectSingleNode("Rivers");
            if (riversNode != null)
            {
                var riverList = new List<WaterFlow>();
                foreach (XmlNode rn in riversNode.ChildNodes)
                    riverList.Add(new WaterFlow(WaterItemType.River, rn));
                Rivers = riverList.ToArray();
            }

            var lakesNode = node.SelectSingleNode("Lakes");
            if (lakesNode != null)
            {
                var lakeList = new List<WaterFlow>();
                foreach (XmlNode ln in lakesNode.ChildNodes)
                    lakeList.Add(new WaterFlow(WaterItemType.Lake, ln));
                Lakes = lakeList.ToArray();
            }

            var poolsNode = node.SelectSingleNode("Pools");
            if (poolsNode != null)
            {
                var poolList = new List<WaterPool>();
                foreach (XmlNode pn in poolsNode.ChildNodes)
                    poolList.Add(new WaterPool(pn));
                Pools = poolList.ToArray();
            }

            RiverVecs = Xml.GetChildVector4Array(node, "RiverVecs");
            LakeVecs = Xml.GetChildVector4Array(node, "LakeVecs");

            var colorsRaw = Xml.GetChildRawUIntArray(node, "Colours");
            if (colorsRaw != null)
                Colours = colorsRaw.Select(c => Color.FromAbgr(c)).ToArray();
        }



        public struct CompHeader
        {
            public byte Start { get; set; }
            public byte Count { get; set; }
            public ushort Offset { get; set; }

            public void Read(DataReader r)
            {
                Start = r.ReadByte();
                Count = r.ReadByte();
                Offset = r.ReadUInt16();
            }

            public void Write(DataWriter w)
            {
                w.Write(Start);
                w.Write(Count);
                w.Write(Offset);
            }

            public void WriteXml(StringBuilder sb, int indent)
            {
                WatermapXml.SelfClosingTag(sb, indent, $"CompHeader Start=\"{Start}\" Count=\"{Count}\" Offset=\"{Offset}\"");
            }

            public static CompHeader ReadXml(XmlNode node)
            {
                return new CompHeader
                {
                    Start = (byte)Xml.GetUIntAttribute(node, "Start"),
                    Count = (byte)Xml.GetUIntAttribute(node, "Count"),
                    Offset = (ushort)Xml.GetUIntAttribute(node, "Offset")
                };
            }

            public override string ToString()
            {
                return $"{Start}, {Count}, {Offset}";
            }
        }


        public struct WaterItemRef
        {
            public ushort RawValue { get; set; }
            public WaterItem Item { get; set; } // link

            public bool EndOfList => ((RawValue >> 15) & 0x1) == 1;
            public WaterItemType Type => (WaterItemType)((RawValue >> 13) & 0x3);

            public ushort ItemIndex =>
                (Type == WaterItemType.River || Type == WaterItemType.Lake)
                    ? (ushort)((RawValue >> 7) & 0x3F)
                    : (ushort)(RawValue & 0x7FF);

            public ushort VectorIndex =>
                (Type == WaterItemType.River || Type == WaterItemType.Lake)
                    ? (ushort)(RawValue & 0x7F)
                    : (ushort)0;

            public Vector4 Vector
            {
                get
                {
                    if (Item?.Vectors == null) return Vector4.Zero;
                    if (VectorIndex >= Item.Vectors.Length) return Vector4.Zero;
                    return Item.Vectors[VectorIndex];
                }
            }

            public WaterItemRef(ushort rawval)
            {
                RawValue = rawval;
                Item = null;
            }

            public void Write(DataWriter w) => w.Write(RawValue);

            public void WriteXml(StringBuilder sb, int indent)
            {
                WatermapXml.SelfClosingTag(
                    sb, indent,
                    $"WaterItemRef Raw=\"0x{RawValue:X4}\" Type=\"{Type}\" EndOfList=\"{EndOfList}\" ItemIndex=\"{ItemIndex}\" VectorIndex=\"{VectorIndex}\""
                );
            }

            public static WaterItemRef ReadXml(XmlNode node)
            {
                var raw = (ushort)Xml.GetUIntAttribute(node, "Raw");
                return new WaterItemRef(raw);
            }

            public override string ToString()
            {
                if (Item != null)
                    return $"{Item}: {Vector}";
                return $"{Type}: {ItemIndex}:{VectorIndex}";
            }
        }
        public enum WaterItemType
        {
            None = 0,
            River = 1,
            Lake = 2,
            Pool = 3,
        }
        public abstract class WaterItem
        {
            public Vector3 Position { get; set; }
            public uint Unk04 { get; set; }
            public Vector3 Size { get; set; }
            public uint Unk09 { get; set; }

            public WaterItemType Type { get; private set; }
            public Vector4[] Vectors { get; set; }
            public Color Colour { get; set; }

            protected WaterItem(WaterItemType type)
            {
                Type = type;
            }

            public virtual void Read(DataReader r)
            {
                Position = r.ReadVector3();
                Unk04 = r.ReadUInt32();
                Size = r.ReadVector3();
                Unk09 = r.ReadUInt32();
            }

            public virtual void Write(DataWriter w)
            {
                w.Write(Position);
                w.Write(Unk04);
                w.Write(Size);
                w.Write(Unk09);
            }

            public virtual void WriteXml(StringBuilder sb, int indent, string tagName = null)
            {
                var name = tagName ?? Type.ToString();
                WatermapXml.OpenTag(sb, indent, name);
                WatermapXml.SelfClosingTag(sb, indent + 1, $"Position {FloatUtil.GetVector3XmlString(Position)}");
                WatermapXml.ValueTag(sb, indent + 1, "Unk04", $"0x{Unk04:X8}");
                WatermapXml.SelfClosingTag(sb, indent + 1, $"Size {FloatUtil.GetVector3XmlString(Size)}");
                WatermapXml.ValueTag(sb, indent + 1, "Unk09", $"0x{Unk09:X8}");
                WatermapXml.ValueTag(sb, indent + 1, "Colour", $"0x{Colour.ToRgba():X8}");
                WatermapXml.CloseTag(sb, indent, name);
            }

            public virtual void ReadXml(XmlNode node)
            {
                Position = Xml.GetChildVector3Attributes(node, "Position");
                Unk04 = Xml.GetChildUIntAttribute(node, "Unk04");
                Size = Xml.GetChildVector3Attributes(node, "Size");
                Unk09 = Xml.GetChildUIntAttribute(node, "Unk09");

                var colStr = Xml.GetChildInnerText(node, "Colour");
                if (!string.IsNullOrEmpty(colStr))
                {
                    if (colStr.StartsWith("0x"))
                        Colour = Color.FromRgba(Convert.ToInt32(colStr, 16));
                    else
                        Colour = Color.FromRgba(int.Parse(colStr));
                }
            }

            public override string ToString()
            {
                return $"{Type} - Size: {Size}, Pos: {Position}";
            }
        }
        public class WaterFlow : WaterItem
        {
            public byte VectorCount { get; set; }
            public byte Unk11 { get; set; }
            public ushort VectorOffset { get; set; }
            public uint Unk13 { get; set; }
            public uint Unk14 { get; set; }
            public uint Unk15 { get; set; }

            public WaterFlow(WaterItemType type) : base(type) { }

            public WaterFlow(WaterItemType type, DataReader r, Vector4[] vecs) : base(type)
            {
                Read(r);
                if (VectorCount > 0)
                {
                    Vectors = new Vector4[VectorCount];
                    for (int i = 0; i < VectorCount; i++)
                        Vectors[i] = vecs[VectorOffset + i];
                }
            }

            public override void Read(DataReader r)
            {
                base.Read(r);
                VectorCount = r.ReadByte();
                Unk11 = r.ReadByte();
                VectorOffset = r.ReadUInt16();
                Unk13 = r.ReadUInt32();
                Unk14 = r.ReadUInt32();
                Unk15 = r.ReadUInt32();
            }

            public override void Write(DataWriter w)
            {
                base.Write(w);
                w.Write(VectorCount);
                w.Write(Unk11);
                w.Write(VectorOffset);
                w.Write(Unk13);
                w.Write(Unk14);
                w.Write(Unk15);
            }

            public override void WriteXml(StringBuilder sb, int indent, string tagName = null)
            {
                var name = tagName ?? Type.ToString();
                WatermapXml.OpenTag(sb, indent, name);
                WatermapXml.SelfClosingTag(sb, indent + 1, $"Position {FloatUtil.GetVector3XmlString(Position)}");
                WatermapXml.SelfClosingTag(sb, indent + 1, $"Size {FloatUtil.GetVector3XmlString(Size)}");
                WatermapXml.ValueTag(sb, indent + 1, "VectorCount", VectorCount.ToString());
                WatermapXml.ValueTag(sb, indent + 1, "VectorOffset", VectorOffset.ToString());
                WatermapXml.ValueTag(sb, indent + 1, "Unk11", $"0x{Unk11:X2}");
                WatermapXml.ValueTag(sb, indent + 1, "Unk13", $"0x{Unk13:X8}");
                WatermapXml.ValueTag(sb, indent + 1, "Unk14", $"0x{Unk14:X8}");
                WatermapXml.ValueTag(sb, indent + 1, "Unk15", $"0x{Unk15:X8}");
                WatermapXml.ValueTag(sb, indent + 1, "Colour", $"0x{Colour.ToRgba():X8}");
                WatermapXml.CloseTag(sb, indent, name);
            }

            public WaterFlow(WaterItemType type, XmlNode node) : base(type)
            {
                ReadXml(node);
                VectorCount = (byte)Xml.GetChildUIntAttribute(node, "VectorCount");
                VectorOffset = (ushort)Xml.GetChildUIntAttribute(node, "VectorOffset");
                Unk11 = (byte)Xml.GetChildUIntAttribute(node, "Unk11");
                Unk13 = Xml.GetChildUIntAttribute(node, "Unk13");
                Unk14 = Xml.GetChildUIntAttribute(node, "Unk14");
                Unk15 = Xml.GetChildUIntAttribute(node, "Unk15");
            }
        }
        public class WaterPool : WaterItem
        {
            public WaterPool() : base(WaterItemType.Pool) { }
            public WaterPool(DataReader r) : base(WaterItemType.Pool) { Read(r); }
            public WaterPool(XmlNode node) : base(WaterItemType.Pool) { ReadXml(node); }

            public override void WriteXml(StringBuilder sb, int indent, string tagName = null)
            {
                var name = tagName ?? "Pool";
                base.WriteXml(sb, indent, name);
            }

            public override string ToString() => base.ToString();
        }




        public string GetPGM()
        {
            if (GridWatermapInds == null) return string.Empty;

            var sb = new StringBuilder();
            sb.AppendFormat("P2\n{0} {1}\n65535\n", Width, Height);
            //sb.AppendFormat("P2\n{0} {1}\n255\n", Width, Height);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var h = GridWatermapInds[y * Width + x];
                    sb.Append(h.ToString());
                    sb.Append(" ");
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }



    }


    public class WatermapXml : MetaXmlBase
    {
        public static string GetXml(WatermapFile wmf)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(XmlHeader);

            if (wmf != null && wmf.CompHeaders != null)
            {
                var name = "Watermap";
                OpenTag(sb, 0, name);

                wmf.WriteXml(sb, 1);

                CloseTag(sb, 0, name);
            }
            return sb.ToString();
        }
    }


    public class XmlWatermap
    {
        public static WatermapFile GetWatermap(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return GetWatermap(doc);
        }

        public static WatermapFile GetWatermap(XmlDocument doc)
        {
            var wmf = new WatermapFile();
            wmf.ReadXml(doc.DocumentElement);
            return wmf;
        }
    }

}
