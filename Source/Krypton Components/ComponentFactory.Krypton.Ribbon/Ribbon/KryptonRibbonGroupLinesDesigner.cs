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

using System;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Diagnostics;
using ComponentFactory.Krypton.Toolkit;

namespace ComponentFactory.Krypton.Ribbon
{
    internal class KryptonRibbonGroupLinesDesigner : ComponentDesigner
    {
        #region Instance Fields
        private IDesignerHost _designerHost;
        private IComponentChangeService _changeService;
        private KryptonRibbonGroupLines _ribbonLines;
        private DesignerVerbCollection _verbs;
        private DesignerVerb _toggleHelpersVerb;
        private DesignerVerb _moveFirstVerb;
        private DesignerVerb _movePrevVerb;
        private DesignerVerb _moveNextVerb;
        private DesignerVerb _moveLastVerb;
        private DesignerVerb _addButtonVerb;
        private DesignerVerb _addColorButtonVerb;
        private DesignerVerb _addCheckBoxVerb;
        private DesignerVerb _addRadioButtonVerb;
        private DesignerVerb _addLabelVerb;
        private DesignerVerb _addCustomControlVerb;
        private DesignerVerb _addClusterVerb;
        private DesignerVerb _addTextBoxVerb;
        private DesignerVerb _addMaskedTextBoxVerb;
        private DesignerVerb _addRichTextBoxVerb;
        private DesignerVerb _addComboBoxVerb;
        private DesignerVerb _addNumericUpDownVerb;
        private DesignerVerb _addDomainUpDownVerb;
        private DesignerVerb _addDateTimePickerVerb;
        private DesignerVerb _addTrackBarVerb;
        private DesignerVerb _clearItemsVerb;
        private DesignerVerb _deleteLinesVerb;
        private ContextMenuStrip _cms;
        private ToolStripMenuItem _toggleHelpersMenu;
        private ToolStripMenuItem _visibleMenu;
        private ToolStripMenuItem _maximumSizeMenu;
        private ToolStripMenuItem _maximumLMenu;
        private ToolStripMenuItem _maximumMMenu;
        private ToolStripMenuItem _maximumSMenu;
        private ToolStripMenuItem _minimumSizeMenu;
        private ToolStripMenuItem _minimumLMenu;
        private ToolStripMenuItem _minimumMMenu;
        private ToolStripMenuItem _minimumSMenu;
        private ToolStripMenuItem _moveFirstMenu;
        private ToolStripMenuItem _movePreviousMenu;
        private ToolStripMenuItem _moveNextMenu;
        private ToolStripMenuItem _moveLastMenu;
        private ToolStripMenuItem _moveToGroupMenu;
        private ToolStripMenuItem _addButtonMenu;
        private ToolStripMenuItem _addColorButtonMenu;
        private ToolStripMenuItem _addCheckBoxMenu;
        private ToolStripMenuItem _addRadioButtonMenu;
        private ToolStripMenuItem _addLabelMenu;
        private ToolStripMenuItem _addCustomControlMenu;
        private ToolStripMenuItem _addClusterMenu;
        private ToolStripMenuItem _addTextBoxMenu;
        private ToolStripMenuItem _addMaskedTextBoxMenu;
        private ToolStripMenuItem _addRichTextBoxMenu;
        private ToolStripMenuItem _addComboBoxMenu;
        private ToolStripMenuItem _addNumericUpDownMenu;
        private ToolStripMenuItem _addDomainUpDownMenu;
        private ToolStripMenuItem _addDateTimePickerMenu;
        private ToolStripMenuItem _addTrackBarMenu;
        private ToolStripMenuItem _clearItemsMenu;
        private ToolStripMenuItem _deleteLinesMenu;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the KryptonRibbonGroupLinesDesigner class.
        /// </summary>
        public KryptonRibbonGroupLinesDesigner()
        {
        }            
        #endregion

        #region Public
        /// <summary>
        /// Initializes the designer with the specified component.
        /// </summary>
        /// <param name="component">The IComponent to associate the designer with.</param>
        public override void Initialize(IComponent component)
        {
            Debug.Assert(component != null);

            // Validate the parameter reference
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            // Let base class do standard stuff
            base.Initialize(component);

            // Cast to correct type
            _ribbonLines = (KryptonRibbonGroupLines)component;
            _ribbonLines.DesignTimeAddButton += OnAddButton;
            _ribbonLines.DesignTimeAddColorButton += OnAddColorButton;
            _ribbonLines.DesignTimeAddCheckBox += OnAddCheckBox;
            _ribbonLines.DesignTimeAddRadioButton += OnAddRadioButton;
            _ribbonLines.DesignTimeAddLabel += OnAddLabel;
            _ribbonLines.DesignTimeAddCustomControl += OnAddCustomControl;
            _ribbonLines.DesignTimeAddCluster += OnAddCluster;
            _ribbonLines.DesignTimeAddTextBox += OnAddTextBox;
            _ribbonLines.DesignTimeAddMaskedTextBox += OnAddMaskedTextBox;
            _ribbonLines.DesignTimeAddRichTextBox += OnAddRichTextBox;
            _ribbonLines.DesignTimeAddComboBox += OnAddComboBox;
            _ribbonLines.DesignTimeAddNumericUpDown += OnAddNumericUpDown;
            _ribbonLines.DesignTimeAddDomainUpDown += OnAddDomainUpDown;
            _ribbonLines.DesignTimeAddDateTimePicker += OnAddDateTimePicker;
            _ribbonLines.DesignTimeAddTrackBar += OnAddTrackBar;
            _ribbonLines.DesignTimeContextMenu += OnContextMenu;

            // Get access to the services
            _designerHost = (IDesignerHost)GetService(typeof(IDesignerHost));
            _changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));

            // We need to know when we are being removed/changed
            _changeService.ComponentRemoving += OnComponentRemoving;
            _changeService.ComponentChanged += OnComponentChanged;
        }

        /// <summary>
        /// Gets the collection of components associated with the component managed by the designer.
        /// </summary>
        public override ICollection AssociatedComponents
        {
            get
            {
                ArrayList compound = new ArrayList(base.AssociatedComponents);
                compound.AddRange(_ribbonLines.Items);
                return compound;
            }
        }

        /// <summary>
        /// Gets the design-time verbs supported by the component that is associated with the designer.
        /// </summary>
        public override DesignerVerbCollection Verbs
        {
            get
            {
                UpdateVerbStatus();
                return _verbs;
            }
        }
        #endregion

        #region Protected
        /// <summary>
        /// Releases all resources used by the component. 
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    // Unhook from events
                    _ribbonLines.DesignTimeAddButton -= OnAddButton;
                    _ribbonLines.DesignTimeAddColorButton -= OnAddColorButton;
                    _ribbonLines.DesignTimeAddCheckBox -= OnAddCheckBox;
                    _ribbonLines.DesignTimeAddRadioButton -= OnAddRadioButton;
                    _ribbonLines.DesignTimeAddLabel -= OnAddLabel;
                    _ribbonLines.DesignTimeAddCustomControl -= OnAddCustomControl;
                    _ribbonLines.DesignTimeAddCluster -= OnAddCluster;
                    _ribbonLines.DesignTimeAddTextBox -= OnAddTextBox;
                    _ribbonLines.DesignTimeAddMaskedTextBox -= OnAddMaskedTextBox;
                    _ribbonLines.DesignTimeAddRichTextBox -= OnAddRichTextBox;
                    _ribbonLines.DesignTimeAddComboBox -= OnAddComboBox;
                    _ribbonLines.DesignTimeAddNumericUpDown -= OnAddNumericUpDown;
                    _ribbonLines.DesignTimeAddDomainUpDown -= OnAddDomainUpDown;
                    _ribbonLines.DesignTimeAddDateTimePicker -= OnAddDateTimePicker;
                    _ribbonLines.DesignTimeContextMenu -= OnContextMenu;
                    _ribbonLines.DesignTimeAddTrackBar -= OnAddTrackBar;
                    _changeService.ComponentRemoving -= OnComponentRemoving;
                    _changeService.ComponentChanged -= OnComponentChanged;
                }
            }
            finally
            {
                // Must let base class do standard stuff
                base.Dispose(disposing);
            }
        }
        #endregion

        #region Implementation
        private void UpdateVerbStatus()
        {
            // Create verbs first time around
            if (_verbs == null)
            {
                _verbs = new DesignerVerbCollection();
                _toggleHelpersVerb = new DesignerVerb("Toggle Helpers", OnToggleHelpers);
                _moveFirstVerb = new DesignerVerb("Move Lines First", OnMoveFirst);
                _movePrevVerb = new DesignerVerb("Move Lines Previous", OnMovePrevious);
                _moveNextVerb = new DesignerVerb("Move Lines Next", OnMoveNext);
                _moveLastVerb = new DesignerVerb("Move Lines Last", OnMoveLast);
                _addButtonVerb = new DesignerVerb("Add Button", OnAddButton);
                _addColorButtonVerb = new DesignerVerb("Add Color Button", OnAddColorButton);
                _addCheckBoxVerb = new DesignerVerb("Add CheckBox", OnAddCheckBox);
                _addRadioButtonVerb = new DesignerVerb("Add RadioButton", OnAddRadioButton);
                _addLabelVerb = new DesignerVerb("Add Label", OnAddLabel);
                _addCustomControlVerb = new DesignerVerb("Add Custom Control", OnAddCustomControl);
                _addClusterVerb = new DesignerVerb("Add Cluster", OnAddCluster);
                _addRichTextBoxVerb = new DesignerVerb("Add RichTextBox", OnAddRichTextBox);
                _addTextBoxVerb = new DesignerVerb("Add TextBox", OnAddTextBox);
                _addMaskedTextBoxVerb = new DesignerVerb("Add MaskedTextBox", OnAddMaskedTextBox);
                _addComboBoxVerb = new DesignerVerb("Add ComboBox", OnAddComboBox);
                _addNumericUpDownVerb = new DesignerVerb("Add NumericUpDown", OnAddNumericUpDown);
                _addDomainUpDownVerb = new DesignerVerb("Add DomainUpDown", OnAddDomainUpDown);
                _addDateTimePickerVerb = new DesignerVerb("Add DateTimePicker", OnAddDateTimePicker);
                _addTrackBarVerb = new DesignerVerb("Add TrackBar", OnAddTrackBar);
                _clearItemsVerb = new DesignerVerb("Clear Items", OnClearItems);
                _deleteLinesVerb = new DesignerVerb("Delete Lines", OnDeleteLines);
                _verbs.AddRange(new DesignerVerb[] { _toggleHelpersVerb, _moveFirstVerb, _movePrevVerb, _moveNextVerb, _moveLastVerb, 
                                                     _addButtonVerb, _addColorButtonVerb, _addCheckBoxVerb, _addClusterVerb, _addComboBoxVerb, _addCustomControlVerb, _addDateTimePickerVerb, _addDomainUpDownVerb, _addLabelVerb, _addNumericUpDownVerb, _addRadioButtonVerb, _addRichTextBoxVerb, _addTextBoxVerb, _addTrackBarVerb, _addMaskedTextBoxVerb,
                                                     _clearItemsVerb, _deleteLinesVerb });
            }

            bool moveFirst = false;
            bool movePrev = false;
            bool moveNext = false;
            bool moveLast = false;
            bool clearItems = false;

            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                moveFirst = (_ribbonLines.RibbonGroup.Items.IndexOf(_ribbonLines) > 0);
                movePrev = (_ribbonLines.RibbonGroup.Items.IndexOf(_ribbonLines) > 0);
                moveNext = (_ribbonLines.RibbonGroup.Items.IndexOf(_ribbonLines) < (_ribbonLines.RibbonGroup.Items.Count - 1));
                moveLast = (_ribbonLines.RibbonGroup.Items.IndexOf(_ribbonLines) < (_ribbonLines.RibbonGroup.Items.Count - 1));
                clearItems = (_ribbonLines.Items.Count > 0);
            }

            _moveFirstVerb.Enabled = moveFirst;
            _movePrevVerb.Enabled = movePrev;
            _moveNextVerb.Enabled = moveNext;
            _moveLastVerb.Enabled = moveLast;
            _clearItemsVerb.Enabled = clearItems;
        }

        private void OnToggleHelpers(object sender, EventArgs e)
        {
            // Invert the current toggle helper mode
            if (_ribbonLines?.Ribbon != null)
            {
                _ribbonLines.Ribbon.InDesignHelperMode = !_ribbonLines.Ribbon.InDesignHelperMode;
            }
        }

        private void OnMoveFirst(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines MoveFirst");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines.RibbonGroup)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Move position of the lines
                    KryptonRibbonGroup ribbonGroup = _ribbonLines.RibbonGroup;
                    ribbonGroup.Items.Remove(_ribbonLines);
                    ribbonGroup.Items.Insert(0, _ribbonLines);
                    UpdateVerbStatus();

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnMovePrevious(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines MovePrevious");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines.RibbonGroup)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Move position of the triple
                    KryptonRibbonGroup ribbonGroup = _ribbonLines.RibbonGroup;
                    int index = ribbonGroup.Items.IndexOf(_ribbonLines) - 1;
                    index = Math.Max(index, 0);
                    ribbonGroup.Items.Remove(_ribbonLines);
                    ribbonGroup.Items.Insert(index, _ribbonLines);
                    UpdateVerbStatus();

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnMoveNext(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines MoveNext");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines.RibbonGroup)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Move position of the triple
                    KryptonRibbonGroup ribbonGroup = _ribbonLines.RibbonGroup;
                    int index = ribbonGroup.Items.IndexOf(_ribbonLines) + 1;
                    index = Math.Min(index, ribbonGroup.Items.Count - 1);
                    ribbonGroup.Items.Remove(_ribbonLines);
                    ribbonGroup.Items.Insert(index, _ribbonLines);
                    UpdateVerbStatus();

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnMoveLast(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines MoveLast");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines.RibbonGroup)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Move position of the triple
                    KryptonRibbonGroup ribbonGroup = _ribbonLines.RibbonGroup;
                    ribbonGroup.Items.Remove(_ribbonLines);
                    ribbonGroup.Items.Insert(ribbonGroup.Items.Count, _ribbonLines);
                    UpdateVerbStatus();

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddButton(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddButton");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a button item
                    KryptonRibbonGroupButton button = (KryptonRibbonGroupButton)_designerHost.CreateComponent(typeof(KryptonRibbonGroupButton));
                    _ribbonLines.Items.Add(button);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddColorButton(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddColorButton");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a button item
                    KryptonRibbonGroupColorButton button = (KryptonRibbonGroupColorButton)_designerHost.CreateComponent(typeof(KryptonRibbonGroupColorButton));
                    _ribbonLines.Items.Add(button);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddCheckBox(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddCheckBox");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a check box item
                    KryptonRibbonGroupCheckBox checkBox = (KryptonRibbonGroupCheckBox)_designerHost.CreateComponent(typeof(KryptonRibbonGroupCheckBox));
                    _ribbonLines.Items.Add(checkBox);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddRadioButton(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddRadioButton");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a radio button item
                    KryptonRibbonGroupRadioButton radioButton = (KryptonRibbonGroupRadioButton)_designerHost.CreateComponent(typeof(KryptonRibbonGroupRadioButton));
                    _ribbonLines.Items.Add(radioButton);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddLabel(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddLabel");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a label item
                    KryptonRibbonGroupLabel label = (KryptonRibbonGroupLabel)_designerHost.CreateComponent(typeof(KryptonRibbonGroupLabel));
                    _ribbonLines.Items.Add(label);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddCustomControl(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddCustomControl");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a custom control item
                    KryptonRibbonGroupCustomControl cc = (KryptonRibbonGroupCustomControl)_designerHost.CreateComponent(typeof(KryptonRibbonGroupCustomControl));
                    _ribbonLines.Items.Add(cc);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddTextBox(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddTextBox");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a textbox item
                    KryptonRibbonGroupTextBox tb = (KryptonRibbonGroupTextBox)_designerHost.CreateComponent(typeof(KryptonRibbonGroupTextBox));
                    _ribbonLines.Items.Add(tb);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddMaskedTextBox(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddMaskedTextBox");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a masked textbox item
                    KryptonRibbonGroupMaskedTextBox mtb = (KryptonRibbonGroupMaskedTextBox)_designerHost.CreateComponent(typeof(KryptonRibbonGroupMaskedTextBox));
                    _ribbonLines.Items.Add(mtb);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddRichTextBox(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddRichTextBox");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a richtextbox item
                    KryptonRibbonGroupRichTextBox rtb = (KryptonRibbonGroupRichTextBox)_designerHost.CreateComponent(typeof(KryptonRibbonGroupRichTextBox));
                    _ribbonLines.Items.Add(rtb);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddComboBox(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddComboBox");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a combobox item
                    KryptonRibbonGroupComboBox cb = (KryptonRibbonGroupComboBox)_designerHost.CreateComponent(typeof(KryptonRibbonGroupComboBox));
                    _ribbonLines.Items.Add(cb);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddNumericUpDown(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddNumericUpDown");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a numeric up-down item
                    KryptonRibbonGroupNumericUpDown nud = (KryptonRibbonGroupNumericUpDown)_designerHost.CreateComponent(typeof(KryptonRibbonGroupNumericUpDown));
                    _ribbonLines.Items.Add(nud);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddDomainUpDown(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddDomainUpDown");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a domain up-down item
                    KryptonRibbonGroupDomainUpDown nud = (KryptonRibbonGroupDomainUpDown)_designerHost.CreateComponent(typeof(KryptonRibbonGroupDomainUpDown));
                    _ribbonLines.Items.Add(nud);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddDateTimePicker(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddDateTimePicker");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a date time picker item
                    KryptonRibbonGroupDateTimePicker nud = (KryptonRibbonGroupDateTimePicker)_designerHost.CreateComponent(typeof(KryptonRibbonGroupDateTimePicker));
                    _ribbonLines.Items.Add(nud);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddTrackBar(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddTrackBar");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a trackbar item
                    KryptonRibbonGroupTrackBar tb = (KryptonRibbonGroupTrackBar)_designerHost.CreateComponent(typeof(KryptonRibbonGroupTrackBar));
                    _ribbonLines.Items.Add(tb);

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnAddCluster(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines AddCluster");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Get designer to create a cluster with buttons
                    KryptonRibbonGroupCluster cluster = (KryptonRibbonGroupCluster)_designerHost.CreateComponent(typeof(KryptonRibbonGroupCluster));
                    _ribbonLines.Items.Add(cluster);

                    // Get access to the Cluster.Items property
                    MemberDescriptor propertyClusterItems = TypeDescriptor.GetProperties(cluster)["Items"];

                    RaiseComponentChanging(propertyClusterItems);

                    // Get designer to create three new button components
                    KryptonRibbonGroupClusterButton button1 = (KryptonRibbonGroupClusterButton)_designerHost.CreateComponent(typeof(KryptonRibbonGroupClusterButton));
                    KryptonRibbonGroupClusterButton button2 = (KryptonRibbonGroupClusterButton)_designerHost.CreateComponent(typeof(KryptonRibbonGroupClusterButton));
                    cluster.Items.Add(button1);
                    cluster.Items.Add(button2);

                    RaiseComponentChanged(propertyClusterItems, null, null);
                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnClearItems(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines ClearItems");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines)["Items"];

                    RaiseComponentChanging(propertyItems);

                    // Need access to host in order to delete a component
                    IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

                    // We need to remove all the items from the lines group
                    for (int i = _ribbonLines.Items.Count - 1; i >= 0; i--)
                    {
                        KryptonRibbonGroupItem item = _ribbonLines.Items[i];
                        _ribbonLines.Items.Remove(item);
                        host.DestroyComponent(item);
                    }

                    RaiseComponentChanged(propertyItems, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnDeleteLines(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines DeleteLines");

                try
                {
                    // Get access to the Items property
                    MemberDescriptor propertyItems = TypeDescriptor.GetProperties(_ribbonLines.RibbonGroup)["Items"];

                    // Remove the ribbon group from the ribbon tab
                    RaiseComponentChanging(null);
                    RaiseComponentChanging(propertyItems);

                    // Remove the page from the ribbon
                    _ribbonLines.RibbonGroup.Items.Remove(_ribbonLines);

                    // Get designer to destroy it
                    _designerHost.DestroyComponent(_ribbonLines);

                    RaiseComponentChanged(propertyItems, null, null);
                    RaiseComponentChanged(null, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }

        private void OnVisible(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                _changeService.OnComponentChanged(_ribbonLines, null, _ribbonLines.Visible, !_ribbonLines.Visible);
                _ribbonLines.Visible = !_ribbonLines.Visible;
            }
        }

        private void OnMaxLarge(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                _changeService.OnComponentChanged(_ribbonLines, null, _ribbonLines.MaximumSize, GroupItemSize.Large);
                _ribbonLines.MaximumSize = GroupItemSize.Large;
            }
        }

        private void OnMaxMedium(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                _changeService.OnComponentChanged(_ribbonLines, null, _ribbonLines.MaximumSize, GroupItemSize.Medium);
                _ribbonLines.MaximumSize = GroupItemSize.Medium;
            }
        }

        private void OnMaxSmall(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                _changeService.OnComponentChanged(_ribbonLines, null, _ribbonLines.MaximumSize, GroupItemSize.Small);
                _ribbonLines.MaximumSize = GroupItemSize.Small;
            }
        }

        private void OnMinLarge(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                _changeService.OnComponentChanged(_ribbonLines, null, _ribbonLines.MinimumSize, GroupItemSize.Large);
                _ribbonLines.MinimumSize = GroupItemSize.Large;
            }
        }

        private void OnMinMedium(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                _changeService.OnComponentChanged(_ribbonLines, null, _ribbonLines.MinimumSize, GroupItemSize.Medium);
                _ribbonLines.MinimumSize = GroupItemSize.Medium;
            }
        }

        private void OnMinSmall(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                _changeService.OnComponentChanged(_ribbonLines, null, _ribbonLines.MinimumSize, GroupItemSize.Small);
                _ribbonLines.MinimumSize = GroupItemSize.Small;
            }
        }

        private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
        {
            UpdateVerbStatus();
        }

        private void OnComponentRemoving(object sender, ComponentEventArgs e)
        {
            // If our component is being removed
            if (e.Component == _ribbonLines)
            {
                // Need access to host in order to delete a component
                IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

                // We need to remove all items from the lines groups
                for (int j = _ribbonLines.Items.Count - 1; j >= 0; j--)
                {
                    KryptonRibbonGroupItem item = _ribbonLines.Items[j] as KryptonRibbonGroupItem;
                    _ribbonLines.Items.Remove(item);
                    host.DestroyComponent(item);
                }
            }
        }

        private void OnContextMenu(object sender, MouseEventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Create the menu strip the first time around
                if (_cms == null)
                {
                    _cms = new ContextMenuStrip();
                    _toggleHelpersMenu = new ToolStripMenuItem("Design Helpers", null, OnToggleHelpers);
                    _visibleMenu = new ToolStripMenuItem("Visible", null, OnVisible);
                    _maximumLMenu = new ToolStripMenuItem("Large", null, OnMaxLarge);
                    _maximumMMenu = new ToolStripMenuItem("Medium", null, OnMaxMedium);
                    _maximumSMenu = new ToolStripMenuItem("Small", null, OnMaxSmall);
                    _maximumSizeMenu = new ToolStripMenuItem("Maximum Size");
                    _maximumSizeMenu.DropDownItems.AddRange(new ToolStripItem[] { _maximumLMenu, _maximumMMenu, _maximumSMenu });
                    _minimumLMenu = new ToolStripMenuItem("Large", null, OnMinLarge);
                    _minimumMMenu = new ToolStripMenuItem("Medium", null, OnMinMedium);
                    _minimumSMenu = new ToolStripMenuItem("Small", null, OnMinSmall);
                    _minimumSizeMenu = new ToolStripMenuItem("Minimum Size");
                    _minimumSizeMenu.DropDownItems.AddRange(new ToolStripItem[] { _minimumLMenu, _minimumMMenu, _minimumSMenu });
                    _moveFirstMenu = new ToolStripMenuItem("Move Lines First", Properties.Resources.MoveFirst, OnMoveFirst);
                    _movePreviousMenu = new ToolStripMenuItem("Move Lines Previous", Properties.Resources.MovePrevious, OnMovePrevious);
                    _moveNextMenu = new ToolStripMenuItem("Move Lines Next", Properties.Resources.MoveNext, OnMoveNext);
                    _moveLastMenu = new ToolStripMenuItem("Move Lines Last", Properties.Resources.MoveLast, OnMoveLast);
                    _moveToGroupMenu = new ToolStripMenuItem("Move Lines To Group");
                    _addButtonMenu = new ToolStripMenuItem("Add Button", Properties.Resources.KryptonRibbonGroupButton, OnAddButton);
                    _addColorButtonMenu = new ToolStripMenuItem("Add Color Button", Properties.Resources.KryptonRibbonGroupColorButton, OnAddColorButton);
                    _addCheckBoxMenu = new ToolStripMenuItem("Add CheckBox", Properties.Resources.KryptonRibbonGroupCheckBox, OnAddCheckBox);
                    _addRadioButtonMenu = new ToolStripMenuItem("Add RadioButton", Properties.Resources.KryptonRibbonGroupRadioButton, OnAddRadioButton);
                    _addLabelMenu = new ToolStripMenuItem("Add Label", Properties.Resources.KryptonRibbonGroupLabel, OnAddLabel);
                    _addCustomControlMenu = new ToolStripMenuItem("Add Custom Control", Properties.Resources.KryptonRibbonGroupCustomControl, OnAddCustomControl);
                    _addClusterMenu = new ToolStripMenuItem("Add Cluster", Properties.Resources.KryptonRibbonGroupCluster, OnAddCluster);
                    _addTextBoxMenu = new ToolStripMenuItem("Add TextBox", Properties.Resources.KryptonRibbonGroupTextBox, OnAddTextBox);
                    _addMaskedTextBoxMenu = new ToolStripMenuItem("Add MaskedTextBox", Properties.Resources.KryptonRibbonGroupMaskedTextBox, OnAddMaskedTextBox);
                    _addRichTextBoxMenu = new ToolStripMenuItem("Add RichTextBox", Properties.Resources.KryptonRibbonGroupRichTextBox, OnAddRichTextBox);
                    _addComboBoxMenu = new ToolStripMenuItem("Add ComboBox", Properties.Resources.KryptonRibbonGroupComboBox, OnAddComboBox);
                    _addNumericUpDownMenu = new ToolStripMenuItem("Add NumericUpDown", Properties.Resources.KryptonRibbonGroupNumericUpDown, OnAddNumericUpDown);
                    _addDomainUpDownMenu = new ToolStripMenuItem("Add DomainUpDown", Properties.Resources.KryptonRibbonGroupDomainUpDown, OnAddDomainUpDown);
                    _addDateTimePickerMenu = new ToolStripMenuItem("Add DateTimePicker", Properties.Resources.KryptonRibbonGroupDateTimePicker, OnAddDateTimePicker);
                    _addTrackBarMenu = new ToolStripMenuItem("Add TrackBar", Properties.Resources.KryptonRibbonGroupTrackBar, OnAddTrackBar);
                    _clearItemsMenu = new ToolStripMenuItem("Clear Items", null, OnClearItems);
                    _deleteLinesMenu = new ToolStripMenuItem("Delete Lines", Properties.Resources.delete2, OnDeleteLines);
                    _cms.Items.AddRange(new ToolStripItem[] { _toggleHelpersMenu, new ToolStripSeparator(),
                                                              _visibleMenu, _maximumSizeMenu, _minimumSizeMenu, new ToolStripSeparator(),
                                                              _moveFirstMenu, _movePreviousMenu, _moveNextMenu, _moveLastMenu, new ToolStripSeparator(),
                                                              _moveToGroupMenu, new ToolStripSeparator(),
                                                              _addButtonMenu, _addColorButtonMenu, _addCheckBoxMenu, _addClusterMenu, _addComboBoxMenu, _addCustomControlMenu, _addDateTimePickerMenu, _addDomainUpDownMenu, _addLabelMenu, _addNumericUpDownMenu, _addRadioButtonMenu, _addRichTextBoxMenu, _addTextBoxMenu, _addTrackBarMenu, _addMaskedTextBoxMenu, new ToolStripSeparator(),
                                                              _clearItemsMenu, new ToolStripSeparator(),
                                                              _deleteLinesMenu });

                    // Ensure add images have correct transparent background
                    _addButtonMenu.ImageTransparentColor = Color.Magenta;
                    _addColorButtonMenu.ImageTransparentColor = Color.Magenta;
                    _addCheckBoxMenu.ImageTransparentColor = Color.Magenta;
                    _addRadioButtonMenu.ImageTransparentColor = Color.Magenta;
                    _addLabelMenu.ImageTransparentColor = Color.Magenta;
                    _addCustomControlMenu.ImageTransparentColor = Color.Magenta;
                    _addClusterMenu.ImageTransparentColor = Color.Magenta;
                    _addTextBoxMenu.ImageTransparentColor = Color.Magenta;
                    _addMaskedTextBoxMenu.ImageTransparentColor = Color.Magenta;
                    _addRichTextBoxMenu.ImageTransparentColor = Color.Magenta;
                    _addComboBoxMenu.ImageTransparentColor = Color.Magenta;
                    _addNumericUpDownMenu.ImageTransparentColor = Color.Magenta;
                    _addDomainUpDownMenu.ImageTransparentColor = Color.Magenta;
                    _addDateTimePickerMenu.ImageTransparentColor = Color.Magenta;
                    _addTrackBarMenu.ImageTransparentColor = Color.Magenta;
                }

                // Update verbs to work out correct enable states
                UpdateVerbStatus();

                // Update sub menu options available for the 'Move To Group'
                UpdateMoveToGroup();

                // Update menu items state from versb
                _toggleHelpersMenu.Checked = _ribbonLines.Ribbon.InDesignHelperMode;
                _visibleMenu.Checked = _ribbonLines.Visible;
                _maximumLMenu.Checked = (_ribbonLines.MaximumSize == GroupItemSize.Large);
                _maximumMMenu.Checked = (_ribbonLines.MaximumSize == GroupItemSize.Medium);
                _maximumSMenu.Checked = (_ribbonLines.MaximumSize == GroupItemSize.Small);
                _minimumLMenu.Checked = (_ribbonLines.MinimumSize == GroupItemSize.Large);
                _minimumMMenu.Checked = (_ribbonLines.MinimumSize == GroupItemSize.Medium);
                _minimumSMenu.Checked = (_ribbonLines.MinimumSize == GroupItemSize.Small);
                _moveFirstMenu.Enabled = _moveFirstVerb.Enabled;
                _movePreviousMenu.Enabled = _movePrevVerb.Enabled;
                _moveNextMenu.Enabled = _moveNextVerb.Enabled;
                _moveLastMenu.Enabled = _moveLastVerb.Enabled;
                _moveToGroupMenu.Enabled = (_moveToGroupMenu.DropDownItems.Count > 0);
                _clearItemsMenu.Enabled = _clearItemsVerb.Enabled;

                // Show the context menu
                if (CommonHelper.ValidContextMenuStrip(_cms))
                {
                    Point screenPt = _ribbonLines.Ribbon.ViewRectangleToPoint(_ribbonLines.LinesView);
                    VisualPopupManager.Singleton.ShowContextMenuStrip(_cms, screenPt);
                }
            }
        }

        private void UpdateMoveToGroup()
        {
            // Remove any existing child items
            _moveToGroupMenu.DropDownItems.Clear();

            if (_ribbonLines.Ribbon != null)
            {
                // Create a new item per group in the same ribbon tab
                foreach (KryptonRibbonGroup group in _ribbonLines.RibbonTab.Groups)
                {
                    // Cannot move to ourself, so ignore outself
                    if (group != _ribbonLines.RibbonGroup)
                    {
                        // Create menu item for the group
                        ToolStripMenuItem groupMenuItem = new ToolStripMenuItem
                        {
                            Text = group.TextLine1 + " " + group.TextLine2,
                            Tag = group
                        };

                        // Hook into selection of the menu item
                        groupMenuItem.Click += OnMoveToGroup;

                        // Add to end of the list of options
                        _moveToGroupMenu.DropDownItems.Add(groupMenuItem);
                    }
                }
            }
        }

        private void OnMoveToGroup(object sender, EventArgs e)
        {
            if ((_ribbonLines?.Ribbon != null) && _ribbonLines.RibbonGroup.Items.Contains(_ribbonLines))
            {
                // Cast to correct type
                ToolStripMenuItem groupMenuItem = (ToolStripMenuItem)sender;

                // Get access to the destination tab
                KryptonRibbonGroup destination = (KryptonRibbonGroup)groupMenuItem.Tag;

                // Use a transaction to support undo/redo actions
                DesignerTransaction transaction = _designerHost.CreateTransaction("KryptonRibbonGroupLines MoveLinesToGroup");

                try
                {
                    // Get access to the Groups property
                    MemberDescriptor oldItems = TypeDescriptor.GetProperties(_ribbonLines.RibbonGroup)["Items"];
                    MemberDescriptor newItems = TypeDescriptor.GetProperties(destination)["Items"];

                    // Remove the ribbon tab from the ribbon
                    RaiseComponentChanging(null);
                    RaiseComponentChanging(oldItems);
                    RaiseComponentChanging(newItems);

                    // Remove group from current group
                    _ribbonLines.RibbonGroup.Items.Remove(_ribbonLines);

                    // Append to the new destination group
                    destination.Items.Add(_ribbonLines);

                    RaiseComponentChanged(newItems, null, null);
                    RaiseComponentChanged(oldItems, null, null);
                    RaiseComponentChanged(null, null, null);
                }
                finally
                {
                    // If we managed to create the transaction, then do it
                    transaction?.Commit();
                }
            }
        }
        #endregion
    }
}
