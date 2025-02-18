﻿#region BSD License
/*
 * 
 * Original BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
 *  © Component Factory Pty Ltd, 2006 - 2016, (Version 4.5.0.0) All rights reserved.
 * 
 *  New BSD 3-Clause License (https://github.com/Krypton-Suite/Standard-Toolkit/blob/master/LICENSE)
 *  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV), et al. 2017 - 2022. All rights reserved. 
 *  
 */
#endregion


namespace Krypton.Toolkit
{
    /// <summary>
    /// Take into account the current theme when creating an Office 2003 appearance.
    /// </summary>
    public class PaletteProfessionalOffice2003 : PaletteProfessionalSystem
    {
        #region Static Fields
        private static readonly Color[] _colorsB = { Color.FromArgb( 89, 135, 214),   // Header1Begin
                                                                 Color.FromArgb(  4,  57, 148) // Header1End
                                                               };

        private static readonly Color[] _colorsG = { Color.FromArgb(175, 192, 130),   // Header1Begin
                                                                 Color.FromArgb( 99, 122,  69) // Header1End  
                                                               };

        private static readonly Color[] _colorsS = { Color.FromArgb(168, 167, 191),   // Header1Begin
                                                                 Color.FromArgb(113, 112, 145) // Header1End
                                                               };
        #endregion

        #region Instance Fields
        private bool _usingOffice2003;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the PaletteProfessionalOffice2003 class.
        /// </summary>
        public PaletteProfessionalOffice2003()
        {
        }
        #endregion

        #region ColorTable
        /// <summary>
        /// Generate an appropriate color table.
        /// </summary>
        /// <returns>KryptonColorTable instance.</returns>
        internal override KryptonProfessionalKCT GenerateColorTable(bool _)
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                // Are visual styles being used in this application?
                if (VisualStyleInformation.IsEnabledByUser)
                {
                    // Is a predefined scheme being used?
                    switch (VisualStyleInformation.ColorScheme)
                    {
                        case @"NormalColor":
                            _usingOffice2003 = true;
                            return new KryptonProfessionalKCT(_colorsB, false, this);
                        case @"HomeStead":
                            _usingOffice2003 = true;
                            return new KryptonProfessionalKCT(_colorsG, false, this);
                        case @"Metallic":
                            _usingOffice2003 = true;
                            return new KryptonProfessionalKCT(_colorsS, false, this);
                    }
                }
            }

            // Not using a recognized office 2003 color scheme
            _usingOffice2003 = false;

            // Not a recognized scheme, so get the base class to generate something 
            // that looks sensible based on the current system settings
            return base.GenerateColorTable(true);
        }
        #endregion

        #region StandardBack
        /// <summary>
        /// Gets the color background drawing style.
        /// </summary>
        /// <param name="style">Background style.</param>
        /// <param name="state">Palette value should be applicable to this state.</param>
        /// <returns>Color drawing style.</returns>
        public override PaletteColorStyle GetBackColorStyle(PaletteBackStyle style, PaletteState state)
        {
            // Only override system palette if a recognized office 2003 color scheme is used
            if (_usingOffice2003)
            {
                switch (style)
                {
                    case PaletteBackStyle.HeaderDockActive:
                    case PaletteBackStyle.HeaderDockInactive:
                        return PaletteColorStyle.Solid;
                }
            }

            return base.GetBackColorStyle(style, state);
        }

        /// <summary>
        /// Gets the first background color.
        /// </summary>
        /// <param name="style">Background style.</param>
        /// <param name="state">Palette value should be applicable to this state.</param>
        /// <returns>Color value.</returns>
        public override Color GetBackColor1(PaletteBackStyle style, PaletteState state)
        {
            // Only override system palette if a recognized office 2003 color scheme is used
            if (_usingOffice2003)
            {
                switch (style)
                {
                    case PaletteBackStyle.ContextMenuItemHighlight:
                        switch (state)
                        {
                            case PaletteState.Disabled:
                                return SystemColors.Control;
                            case PaletteState.Normal:
                                return Color.Empty;
                            case PaletteState.Tracking:
                                return ColorTable.MenuItemSelectedGradientBegin;
                        }
                        break;
                    case PaletteBackStyle.HeaderDockInactive:
                        return state == PaletteState.Disabled ? SystemColors.Control : ColorTable.ButtonCheckedHighlight;

                    case PaletteBackStyle.HeaderDockActive:
                        return state == PaletteState.Disabled ? SystemColors.Control : SystemColors.Highlight;
                }
            }

            return base.GetBackColor1(style, state);
        }

        /// <summary>
        /// Gets the second back color.
        /// </summary>
        /// <param name="style">Background style.</param>
        /// <param name="state">Palette value should be applicable to this state.</param>
        /// <returns>Color value.</returns>
        public override Color GetBackColor2(PaletteBackStyle style, PaletteState state)
        {
            // Only override system palette if a recognized office 2003 color scheme is used
            if (_usingOffice2003)
            {
                switch (style)
                {
                    case PaletteBackStyle.ContextMenuItemHighlight:
                        switch (state)
                        {
                            case PaletteState.Disabled:
                                return SystemColors.Control;
                            case PaletteState.Normal:
                                return Color.Empty;
                            case PaletteState.Tracking:
                                return ColorTable.MenuItemSelectedGradientBegin;
                        }
                        break;
                    case PaletteBackStyle.HeaderDockInactive:
                        return state == PaletteState.Disabled ? SystemColors.Control : ColorTable.ButtonCheckedHighlight;

                    case PaletteBackStyle.HeaderDockActive:
                        return state == PaletteState.Disabled ? SystemColors.Control : SystemColors.Highlight;

                    case PaletteBackStyle.TabDock:
                        switch (state)
                        {
                            case PaletteState.Disabled:
                                return SystemColors.Control;
                            case PaletteState.Normal:
                                return MergeColors(SystemColors.Window, 0.1f, ColorTable.ButtonCheckedHighlight, 0.9f);
                            case PaletteState.Pressed:
                            case PaletteState.Tracking:
                                return MergeColors(SystemColors.Window, 0.4f, ColorTable.ButtonCheckedGradientMiddle, 0.6f);
                            case PaletteState.CheckedNormal:
                            case PaletteState.CheckedPressed:
                            case PaletteState.CheckedTracking:
                                return SystemColors.Window;
                        }
                        break;
                }
            }

            return base.GetBackColor2(style, state);
        }
        #endregion

        #region StandardContent
        /// <summary>
        /// Gets the first back color for the short text.
        /// </summary>
        /// <param name="style">Content style.</param>
        /// <param name="state">Palette value should be applicable to this state.</param>
        /// <returns>Color value.</returns>
        public override Color GetContentShortTextColor1(PaletteContentStyle style, PaletteState state)
        {
            // Only override system palette if a recognized office 2003 color scheme is used
            if (_usingOffice2003)
            {
                if (state == PaletteState.Disabled)
                {
                    return SystemColors.ControlDark;
                }

                switch (style)
                {
                    case PaletteContentStyle.GridHeaderRowList:
                    case PaletteContentStyle.GridHeaderRowSheet:
                    case PaletteContentStyle.GridHeaderRowCustom1:
                    case PaletteContentStyle.GridHeaderRowCustom2:
                    case PaletteContentStyle.GridHeaderRowCustom3:
                    case PaletteContentStyle.GridHeaderColumnList:
                    case PaletteContentStyle.GridHeaderColumnSheet:
                    case PaletteContentStyle.GridHeaderColumnCustom1:
                    case PaletteContentStyle.GridHeaderColumnCustom2:
                    case PaletteContentStyle.GridHeaderColumnCustom3:
                    case PaletteContentStyle.HeaderDockInactive:
                        return SystemColors.ControlText;
                    case PaletteContentStyle.HeaderDockActive:
                        return SystemColors.ActiveCaptionText;
                }
            }

            // Get everything else from the base class implementation
            return base.GetContentShortTextColor1(style, state);
        }

        /// <summary>
        /// Gets the second back color for the short text.
        /// </summary>
        /// <param name="style">Content style.</param>
        /// <param name="state">Palette value should be applicable to this state.</param>
        /// <returns>Color value.</returns>
        public override Color GetContentShortTextColor2(PaletteContentStyle style, PaletteState state)
        {
            // Only override system palette if a recognized office 2003 color scheme is used
            if (_usingOffice2003)
            {
                if (state == PaletteState.Disabled)
                {
                    return SystemColors.ControlDark;
                }

                switch (style)
                {
                    case PaletteContentStyle.HeaderDockInactive:
                        return SystemColors.ControlText;
                    case PaletteContentStyle.HeaderDockActive:
                        return SystemColors.ActiveCaptionText;
                }
            }

            // Get everything else from the base class implementation
            return base.GetContentShortTextColor2(style, state);
        }
        #endregion
    }
}
