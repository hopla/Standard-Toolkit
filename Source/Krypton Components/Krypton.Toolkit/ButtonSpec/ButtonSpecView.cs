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
    /// Create and manage the view for a ButtonSpec definition.
    /// </summary>
    public class ButtonSpecView : GlobalId,
                                  IContentValues
    {
        #region Instance Fields
        private readonly PaletteRedirect _redirector;
        private readonly PaletteTripleRedirect _palette;
        private readonly EventHandler _finishDelegate;
        private ButtonController _controller;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the ButtonSpecView class.
        /// </summary>
        /// <param name="redirector">Palette redirector.</param>
        /// <param name="paletteMetric">Source for metric values.</param>
        /// <param name="metricPadding">Padding metric for border padding.</param>
        /// <param name="manager">Reference to owning manager.</param>
        /// <param name="buttonSpec">Access</param>
        public ButtonSpecView(PaletteRedirect redirector,
                              IPaletteMetric paletteMetric,
                              PaletteMetricPadding metricPadding,
                              ButtonSpecManagerBase manager,
                              ButtonSpec buttonSpec)
        {
            Debug.Assert(redirector != null);
            Debug.Assert(manager != null);
            Debug.Assert(buttonSpec != null);

            // Remember references
            _redirector = redirector;
            Manager = manager;
            ButtonSpec = buttonSpec;
            _finishDelegate = OnFinishDelegate;

            // Create delegate for paint notifications
            NeedPaintHandler needPaint = OnNeedPaint;

            // Intercept calls from the button for color remapping and instead use
            // the button spec defined map and the container foreground color
            RemapPalette = Manager.CreateButtonSpecRemap(redirector, buttonSpec);

            // Use a redirector to get button values directly from palette
            _palette = new PaletteTripleRedirect(RemapPalette,
                                                 PaletteBackStyle.ButtonButtonSpec,
                                                 PaletteBorderStyle.ButtonButtonSpec,
                                                 PaletteContentStyle.ButtonButtonSpec,
                                                 needPaint);


            // Create the view for displaying a button
            ViewButton = new ViewDrawButton(_palette, _palette, _palette, _palette,
                                             paletteMetric, this, VisualOrientation.Top, false);

            // Associate the view with the source component (for design time support)
            if (buttonSpec.AllowComponent)
            {
                ViewButton.Component = buttonSpec;
            }

            // Use a view center to place button in centre of given space
            ViewCenter = new ViewLayoutCenter(paletteMetric, metricPadding, VisualOrientation.Top)
            {
                ViewButton
            };

            // Create a controller for managing button behavior
            ButtonSpecViewControllers controllers = CreateController(ViewButton, needPaint, OnClick);
            ViewButton.MouseController = controllers.MouseController;
            ViewButton.SourceController = controllers.SourceController;
            ViewButton.KeyController = controllers.KeyController;

            // We need notifying whenever a button specification property changes
            ButtonSpec.ButtonSpecPropertyChanged += OnPropertyChanged;

            // Associate the button spec with the view that is drawing it
            ButtonSpec.SetView(ViewButton);

            // Finally update view with current button spec settings
            UpdateButtonStyle();
            UpdateVisible();
            UpdateEnabled();
            UpdateChecked();
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets access to the owning manager.
        /// </summary>
        public ButtonSpecManagerBase Manager { get; }

        /// <summary>
        /// Gets access to the monitored button spec
        /// </summary>
        public ButtonSpec ButtonSpec { get; }

        /// <summary>
        /// Gets access to the view centering that contains the button.
        /// </summary>
        public ViewLayoutCenter ViewCenter { get; }

        /// <summary>
        /// Gets access to the view centering that contains the button.
        /// </summary>
        public ViewDrawButton ViewButton { get; }

        /// <summary>
        /// Gets access to the remapping palette.
        /// </summary>
        public PaletteRedirect RemapPalette { get; }

        /// <summary>
        /// Gets and sets the composition setting for the button.
        /// </summary>
        public bool DrawButtonSpecOnComposition
        {
            get => ViewButton.DrawButtonComposition;
            set => ViewButton.DrawButtonComposition = value;
        }

        /// <summary>
        /// Requests a repaint and optional layout be performed.
        /// </summary>
        /// <param name="needLayout">Does the palette change require a layout.</param>
        public void PerformNeedPaint(bool needLayout) => Manager.PerformNeedPaint(this, needLayout);

        /// <summary>
        /// Update the button style to reflect new button style setting.
        /// </summary>
        public void UpdateButtonStyle() => _palette.SetStyles(ButtonSpec.GetStyle(_redirector));

        /// <summary>
        /// Update view button to reflect new button visible setting.
        /// </summary>
        public bool UpdateVisible()
        {
            // Decide if the view should be visible or not
            var prevVisible = ViewCenter.Visible;
            ViewCenter.Visible = ButtonSpec.GetVisible(_redirector);

            // Return if a change has occurred
            return prevVisible != ViewCenter.Visible;
        }

        /// <summary>
        /// Update view button to reflect new button enabled setting.
        /// </summary>
        /// <returns>True is a change in state has occurred.</returns>
        public bool UpdateEnabled()
        {
            var changed = false;

            // Remember the initial state
            ViewBase newDependant;
            bool newEnabled;

            switch (ButtonSpec.GetEnabled(_redirector))
            {
                case ButtonEnabled.True:
                    newDependant = null;
                    newEnabled = true;
                    break;
                case ButtonEnabled.False:
                    newDependant = null;
                    newEnabled = false;
                    break;
                case ButtonEnabled.Container:
                    newDependant = ViewCenter.Parent;
                    newEnabled = true;
                    break;
                default:
                    // Should never happen!
                    Debug.Assert(false);
                    newDependant = null;
                    newEnabled = false;
                    break;
            }

            // Only make change if the values have changed
            if (newEnabled != ViewButton.Enabled)
            {
                ViewButton.Enabled = newEnabled;
                changed = true;
            }

            if (newDependant != ViewButton.DependantEnabledState)
            {
                ViewButton.DependantEnabledState = newDependant;
                changed = true;
            }

            return changed;
        }

        /// <summary>
        /// Update view button to reflect new button checked setting.
        /// </summary>
        /// <returns>True is a change in state has occurred.</returns>
        public bool UpdateChecked()
        {
            // Remember the initial state
            bool newChecked;

            switch (ButtonSpec.GetChecked(_redirector))
            {
                case ButtonCheckState.NotCheckButton:
                case ButtonCheckState.Unchecked:
                    newChecked = false;
                    break;
                case ButtonCheckState.Checked:
                    newChecked = true;
                    break;
                default:
                    // Should never happen!
                    Debug.Assert(false);
                    newChecked = false;
                    break;
            }

            // Only make change if the value has changed
            if (newChecked != ViewButton.Checked)
            {
                ViewButton.Checked = newChecked;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Destruct the previously created views.
        /// </summary>
        public void Destruct()
        {
            // Unhook from events
            ButtonSpec.ButtonSpecPropertyChanged -= OnPropertyChanged;

            // Remove buttonspec/view association
            ButtonSpec.SetView(null);

            // Remove all view element resources
            ViewCenter.Dispose();
        }
        #endregion

        #region Protected
        /// <summary>
        /// Create a button controller for the view.
        /// </summary>
        /// <param name="viewButton">View to be controlled.</param>
        /// <param name="needPaint">Paint delegate.</param>
        /// <param name="clickHandler">Reference to click handler.</param>
        /// <returns>Controller instance.</returns>
        public virtual ButtonSpecViewControllers CreateController(ViewDrawButton viewButton,
                                                                  NeedPaintHandler needPaint,
                                                                  MouseEventHandler clickHandler)
        {
            // Create a standard button controller
            _controller = new ButtonController(viewButton, needPaint)
            {
                BecomesFixed = true
            };
            _controller.Click += clickHandler;

            // If associated with a tooltip manager then pass mouse messages onto tooltip manager
            IMouseController mouseController = _controller;
            if (Manager.ToolTipManager != null)
            {
                mouseController = new ToolTipController(Manager.ToolTipManager, viewButton, _controller);
            }

            // Return a collection of controllers
            return new ButtonSpecViewControllers(mouseController, _controller, _controller);
        }

        /// <summary>
        /// Processes the finish of the button being pressed.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnFinishDelegate(object sender, EventArgs e) =>
            // Ask the button to remove the fixed pressed appearance
            _controller.RemoveFixed();

        #endregion

        #region IContentValues

        private float _lastFactorDpiX;
        private float _lastFactorDpiY;
        private readonly Dictionary<Image, Image> _cachedImages = new();
        /// <summary>
        /// Gets the content image.
        /// </summary>
        /// <param name="state">The state for which the image is needed.</param>
        /// <returns>Image value.</returns>
        public Image GetImage(PaletteState state)
        {
            // Get value from button spec passing inheritance redirector
            var baseImage = ButtonSpec.GetImage(_redirector, state);
            if (baseImage == null)
            {
                return null;
            }

            if ( !_cachedImages.ContainsKey(baseImage))
            {
                #region Old Code

                /*// Currently the `ViewButton.FactorDpi#`'s do _NOT_ change whilst the app is running
                _lastFactorDpiX = ViewButton.FactorDpiX;
                _lastFactorDpiY = ViewButton.FactorDpiY;

                var targetWidth = 16 * _lastFactorDpiX * 1.25f;
                var targetHeight = 16 * _lastFactorDpiY * 1.25f;

                if ((targetWidth > baseImage.Width) 
                    && (targetHeight > baseImage.Height)
                    )
                {
                    // Image needs to be regenerated as oversized in order to be scaled back later
                    // $\Standard-Toolkit\Source\Krypton Components\Krypton.Toolkit\Rendering\RenderStandard.cs
                    // line 5779: memento.Image = CommonHelper.ScaleImageForSizedDisplay(memento.Image, currentWidth, currentHeight);
                    var ratio = 1.0f * Math.Min(baseImage.Width, baseImage.Height) /
                                Math.Max(baseImage.Width, baseImage.Height);
                    _cachedImages[baseImage] =
                        CommonHelper.ScaleImageForSizedDisplay(baseImage, baseImage.Width * ratio * _lastFactorDpiX, baseImage.Height * ratio * _lastFactorDpiY);
                }
                else
                {
                    _cachedImages[baseImage] = baseImage;
                }*/

                    #endregion

                // Image needs to be regenerated
                _lastFactorDpiX = ViewButton.FactorDpiX;

                _lastFactorDpiY = ViewButton.FactorDpiY;

                var currentWidth = baseImage.Width * _lastFactorDpiX;

                var currentHeight = baseImage.Height * _lastFactorDpiY;

                if (Math.Abs(_lastFactorDpiY - 1.00f) < 0.001)
                {
                    // Need to workaround the image drawing off the bottom of the title bar when scaling @ 100%
                    // Has to be even to ensure that horizontal lines are still drawn
                    currentHeight -= 2;
                }

                if (currentHeight < 0.75)
                {
                    currentHeight = 1;
                }

                _cachedImages[baseImage] = CommonHelper.ScaleImageForSizedDisplay(baseImage, currentWidth, currentHeight);
            }

            return _cachedImages[baseImage];
        }

        /// <summary>
        /// Gets the content image transparent color.
        /// </summary>
        /// <param name="state">The state for which the image color is needed.</param>
        /// <returns>Color value.</returns>
        public Color GetImageTransparentColor(PaletteState state) =>
            // Get value from button spec passing inheritance redirector
            ButtonSpec.GetImageTransparentColor(_redirector);

        /// <summary>
        /// Gets the content short text.
        /// </summary>
        /// <returns>String value.</returns>
        public string GetShortText() =>
            // Get value from button spec passing inheritance redirector
            ButtonSpec.GetShortText(_redirector);

        /// <summary>
        /// Gets the content long text.
        /// </summary>
        /// <returns>String value.</returns>
        public string GetLongText() =>
            // Get value from button spec passing inheritance redirector
            ButtonSpec.GetLongText(_redirector);

        #endregion

        #region Implementation
        private void OnClick(object sender, MouseEventArgs e)
        {
            // Never show a context menu in design mode
            if (!CommonHelper.DesignMode(Manager.Control))
            {
                // Fire the event handlers hooked into the button spec click event
                ButtonSpec.PerformClick(e);

                // Does the button spec define a krypton context menu?
                if ((ButtonSpec.KryptonContextMenu != null) && (ViewButton != null))
                {
                    // Convert from control coordinates to screen coordinates
                    Rectangle rect = ViewButton.ClientRectangle;

                    // If the button spec is on the chrome titlebar then find position manually
                    Point pt = Manager.Control is Form
                        ? new Point(Manager.Control.Left + rect.Left, Manager.Control.Top + rect.Bottom + 3)
                        : Manager.Control.PointToScreen(new Point(rect.Left, rect.Bottom + 3));

                    // Show the context menu just below the view itself
                    ButtonSpec.KryptonContextMenu.Closed += OnKryptonContextMenuClosed;
                    if (!ButtonSpec.KryptonContextMenu.Show(ButtonSpec, pt))
                    {
                        // Menu not being shown, so clean up
                        ButtonSpec.KryptonContextMenu.Closed -= OnKryptonContextMenuClosed;

                        // Not showing a context menu, so remove the fixed view immediately
                        _finishDelegate?.Invoke(this, EventArgs.Empty);
                    }
                }
                else if ((ButtonSpec.ContextMenuStrip != null) && (ViewButton != null))
                {
                    // Set the correct renderer for the menu strip
                    ButtonSpec.ContextMenuStrip.Renderer = Manager.RenderToolStrip();

                    // Convert from control coordinates to screen coordinates
                    Rectangle rect = ViewButton.ClientRectangle;
                    Point pt = Manager.Control.PointToScreen(new Point(rect.Left, rect.Bottom + 3));

                    // Show the context menu just below the view itself
                    VisualPopupManager.Singleton.ShowContextMenuStrip(ButtonSpec.ContextMenuStrip, pt, _finishDelegate);
                }
                else
                {
                    // Not showing a context menu, so remove the fixed view immediately
                    _finishDelegate?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                // Not showing a context menu, so remove the fixed view immediately
                _finishDelegate?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnKryptonContextMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            // Unhook from context menu event so it could garbage collected in the future
            KryptonContextMenu kcm = (KryptonContextMenu)sender;
            kcm.Closed -= OnKryptonContextMenuClosed;

            // Remove the fixed button appearance
            OnFinishDelegate(sender, e);
        }

        private void OnNeedPaint(object sender, NeedLayoutEventArgs e) => PerformNeedPaint(e.NeedLayout);

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case @"Image":
                case @"Text":
                case @"ExtraText":
                case @"ColorMap":
                    PerformNeedPaint(true);
                    break;
                case @"Style":
                    UpdateButtonStyle();
                    PerformNeedPaint(true);
                    break;
                case @"Visible":
                    UpdateVisible();
                    PerformNeedPaint(true);
                    break;
                case @"Enabled":
                    UpdateEnabled();
                    PerformNeedPaint(true);
                    break;
                case @"Checked":
                    UpdateChecked();
                    PerformNeedPaint(true);
                    break;
            }
        }
        #endregion
    }
}
