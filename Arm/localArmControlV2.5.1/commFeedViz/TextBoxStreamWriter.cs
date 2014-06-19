using System;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Windows.Threading;

namespace commFeedViz {
    public class TextBoxStreamWriter : TextWriter {
        TextBox _output = null;
        commFeedViz CFV;
        public TextBoxStreamWriter(TextBox output, commFeedViz _CFV) {
            _output = output;
            CFV = _CFV;
        }

        public override void Write(char value) {
            base.Write(value);
            CFV.Dispatcher.Invoke(()=>_output.AppendText(value.ToString())); // When character data is written, append it to the text box.
        }

        public override Encoding Encoding {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
