﻿#region BSD License
/*
 * 
 * Original BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
 *  © Component Factory Pty Ltd, 2006 - 2016, All rights reserved.
 * 
 *  New BSD 3-Clause License (https://github.com/Krypton-Suite/Standard-Toolkit/blob/master/LICENSE)
 *  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV), et al. 2017 - 2021. All rights reserved. 
 *  
 *  Modified: Monday 12th April, 2021 @ 18:00 GMT
 *
 */
#endregion

using System;
using System.Windows.Forms;

namespace Krypton.Toolkit
{
    /// <summary>
    /// Image select event data.
    /// </summary>
    public class ImageSelectEventArgs : EventArgs
    {
        #region Instance Fields

        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the ImageSelectEventArgs class.
        /// </summary>
        /// <param name="imageList">Defined image list.</param>
        /// <param name="imageIndex">Index within the image list.</param>
        public ImageSelectEventArgs(ImageList imageList, int imageIndex)
        {
            ImageList = imageList;
            ImageIndex = imageIndex;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets the image list.
        /// </summary>
        public ImageList ImageList { get; }

        /// <summary>
        /// Gets the image index.
        /// </summary>
        public int ImageIndex { get; }

        #endregion
    }
}
