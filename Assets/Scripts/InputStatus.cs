using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Holds information regarding an input's status
    /// </summary>
    public class InputStatus
    {
        public bool receivedInput;
        public bool resetInput;

        /// <summary>
        /// if the resetInput boolean is true then it sets both InputStatus booleans to false
        /// </summary>
        public void Reset()
        {
            receivedInput = false;
            resetInput = false;
        }
    }
}
