﻿using System.Collections.Generic;
using GameNS;
using UnityEngine;
using WorldNS;

namespace SaveSystemNS {
    public struct DataChunk {
        public Sector sector;
        public DataField[] fields;
    }
}