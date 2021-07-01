using System;
using System.Collections.Generic;
using System.Text;

namespace CodingBatExtractor {
    class CodingBatProblem {
        public CodingBatProblem(string problemName, string problemGroup, string link, string description, string implementation) {
            ProblemName = problemName;
            ProblemGroup = problemGroup;
            Link = link;
            Description = description;
            Implementation = implementation;
        }

        public string ProblemName { get; }
        public string ProblemGroup { get; }
        public string Link { get; }
        public string Description { get; }
        public string Implementation { get; }


        public string Format() {
            return 
$@"package {ProblemGroup.Replace('-', '_')};

import java.util.*;
import java.util.stream.*;

public class {ProblemName[0].ToString().ToUpper() + ProblemName.Substring(1)} {{

  /**
   * {Description}
   * Problem Source: {Link}
   */
  {Implementation.Replace("\n", "\n  ")}

}}";
        }
    }
}
