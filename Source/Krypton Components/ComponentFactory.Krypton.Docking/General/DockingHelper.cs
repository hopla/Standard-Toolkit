﻿// *****************************************************************************
// BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
//  © Component Factory Pty Ltd, 2006 - 2016, All rights reserved.
// The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 13 Swallows Close, 
//  Mornington, Vic 3931, Australia and are supplied subject to license terms.
// 
//  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV) 2017 - 2020. All rights reserved. (https://github.com/Krypton-Suite/Standard-Toolkit)
//  Version 5.500.0.0  www.ComponentFactory.com
// *****************************************************************************

using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace ComponentFactory.Krypton.Docking
{
    /// <summary>
    ///  Set of common helper routines for Docking functionality
    /// </summary>
    public static class DockingHelper
    {
        #region Public
        /// <summary>
        /// Convert from DockEdge to DockStyle enumeration value.
        /// </summary>
        /// <param name="edge">DockEdge value to convert.</param>
        /// <param name="opposite">Should the separator be docked against the opposite edge.</param>
        /// <returns>DockStyle value.</returns>
        public static DockStyle DockStyleFromDockEdge(DockingEdge edge, bool opposite)
        {
            switch (edge)
            {
                case DockingEdge.Top:
                    return (opposite ? DockStyle.Bottom : DockStyle.Top);
                case DockingEdge.Bottom:
                    return (opposite ? DockStyle.Top : DockStyle.Bottom);
                case DockingEdge.Left:
                    return (opposite ? DockStyle.Right : DockStyle.Left);
                case DockingEdge.Right:
                    return (opposite ? DockStyle.Left : DockStyle.Right);
                default:
                    // Should never happen!
                    Debug.Assert(false);
                    return DockStyle.Top;
            }
        }

        /// <summary>
        /// Convert the DockEdge to Orientation enumeration value.
        /// </summary>
        /// <param name="edge">DockEdge value to convert.</param>
        /// <returns>Orientation value.</returns>
        public static Orientation OrientationFromDockEdge(DockingEdge edge)
        {
            switch (edge)
            {
                case DockingEdge.Left:
                case DockingEdge.Right:
                    return Orientation.Vertical;
                default:
                    return Orientation.Horizontal;
            }
        }

        /// <summary>
        /// Find the inner space that occupied by the edge docking controls.
        /// </summary>
        /// <param name="c">Reference to control.</param>
        /// <returns>Rectangle in control coordinates.</returns>
        public static Rectangle InnerRectangle(Control c)
        {
            // Start with entire client area
            Rectangle inner = c.ClientRectangle;

            // Adjust for edge docked controls
            foreach (Control child in c.Controls)
            {
                if (child.Visible)
                {
                    switch (child.Dock)
                    {
                        case DockStyle.Left:
                            inner.Width -= child.Width;
                            inner.X += child.Width;
                            break;
                        case DockStyle.Right:
                            inner.Width -= child.Width;
                            break;
                        case DockStyle.Top:
                            inner.Height -= child.Height;
                            inner.Y += child.Height;
                            break;
                        case DockStyle.Bottom:
                            inner.Height -= child.Height;
                            break;
                    }
                }
            }

            return inner;
        }
        #endregion
    }
}
