using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SQLComparator
{
    public partial class SplitterControl
    {

        public delegate void SplitMovedEventHandler();
        public event SplitMovedEventHandler SplitMoved;

        private const int SnapDelta = 20;
        private bool _SplitFixed = true;

        private void SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
        {

            if (IsSplitCentered(new Point(e.SplitX, e.SplitY)))
            {
                _SplitFixed = true;
                Center();
            }
            else
            {
                _SplitFixed = false;
            }
            if (SplitMoved != null)
                SplitMoved();

        }

        public void Center()
        {
            if (Splitter.Height != 0 && Splitter.Width != 0)
            {
                int RelevantDimension = (this.Splitter.Orientation == Orientation.Horizontal ? Splitter.Height : Splitter.Width);
                int ProposedDistance = RelevantDimension / 2;
                if (ProposedDistance > Splitter.Panel1MinSize && ProposedDistance < (RelevantDimension - Splitter.Panel2MinSize))
                    this.Splitter.SplitterDistance = ProposedDistance;
              
            }
        }

        private bool IsSplitCentered(Point Location)
        {

            if (this.Splitter.Orientation == Orientation.Horizontal)
            {
                return Math.Abs(Location.Y - Splitter.Height / 2) < SnapDelta;
            }
            else
            {
                return Math.Abs(Location.X - Splitter.Width / 2) < SnapDelta;
            }

        }

        private void SplitterMoving(object sender, System.Windows.Forms.SplitterCancelEventArgs e)
        {

            Point Location = new Point(e.MouseCursorX, e.MouseCursorY);
            Cursor SplitCursor = ((this.Splitter.Orientation == Orientation.Horizontal) ? Cursors.HSplit : Cursors.VSplit);
            this.Cursor = ((IsSplitCentered(Location)) ? SplitCursor : Cursors.Default);
        }

        private void ComparisonControl_Resize(object sender, System.EventArgs e)
        {
            if (_SplitFixed)
            {
                Center();
            }
        }

    }

} //end of root namespace