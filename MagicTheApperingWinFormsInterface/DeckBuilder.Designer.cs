namespace MagicTheApperingWinFormsInterface
{
    partial class DeckBuilder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeckBuilder));
            this.CardImageBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CardList = new System.Windows.Forms.ListBox();
            this.RulesLabel = new System.Windows.Forms.Label();
            this.ConvertedManaCostLabel = new System.Windows.Forms.Label();
            this.ColorLabel = new System.Windows.Forms.Label();
            this.TypeLabel = new System.Windows.Forms.Label();
            this.RulesBox = new System.Windows.Forms.TextBox();
            this.ConvertedManaCostBox = new System.Windows.Forms.TextBox();
            this.ColorBox = new System.Windows.Forms.TextBox();
            this.TypeBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.AddToDeckButton = new System.Windows.Forms.Button();
            this.ShowDeckButton = new System.Windows.Forms.Button();
            this.CountLabel = new System.Windows.Forms.Label();
            this.RulesAloneLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.ClearButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FileToolStripButton = new System.Windows.Forms.ToolStripSplitButton();
            this.saveDeckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDeckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDeckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createPrintableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.updateCardsDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.RecentSelectionBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ListStatusLabel = new System.Windows.Forms.Label();
            this.SuperTypeLabel = new System.Windows.Forms.Label();
            this.SuperTypeTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.CardImageBox)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CardImageBox
            // 
            this.CardImageBox.Location = new System.Drawing.Point(12, 29);
            this.CardImageBox.Name = "CardImageBox";
            this.CardImageBox.Size = new System.Drawing.Size(429, 401);
            this.CardImageBox.TabIndex = 0;
            this.CardImageBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 436);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Search for?";
            // 
            // CardList
            // 
            this.CardList.FormattingEnabled = true;
            this.CardList.ItemHeight = 16;
            this.CardList.Location = new System.Drawing.Point(697, 61);
            this.CardList.Name = "CardList";
            this.CardList.Size = new System.Drawing.Size(339, 596);
            this.CardList.TabIndex = 3;
            this.CardList.SelectedIndexChanged += new System.EventHandler(this.CardList_SelectedIndexChanged);
            // 
            // RulesLabel
            // 
            this.RulesLabel.AutoSize = true;
            this.RulesLabel.Location = new System.Drawing.Point(11, 468);
            this.RulesLabel.Name = "RulesLabel";
            this.RulesLabel.Size = new System.Drawing.Size(48, 17);
            this.RulesLabel.TabIndex = 4;
            this.RulesLabel.Text = "Rules:";
            // 
            // ConvertedManaCostLabel
            // 
            this.ConvertedManaCostLabel.AutoSize = true;
            this.ConvertedManaCostLabel.Location = new System.Drawing.Point(14, 496);
            this.ConvertedManaCostLabel.Name = "ConvertedManaCostLabel";
            this.ConvertedManaCostLabel.Size = new System.Drawing.Size(140, 17);
            this.ConvertedManaCostLabel.TabIndex = 5;
            this.ConvertedManaCostLabel.Text = "Coverted Mana Cost:";
            // 
            // ColorLabel
            // 
            this.ColorLabel.AutoSize = true;
            this.ColorLabel.Location = new System.Drawing.Point(14, 524);
            this.ColorLabel.Name = "ColorLabel";
            this.ColorLabel.Size = new System.Drawing.Size(45, 17);
            this.ColorLabel.TabIndex = 6;
            this.ColorLabel.Text = "Color:";
            // 
            // TypeLabel
            // 
            this.TypeLabel.AutoSize = true;
            this.TypeLabel.Location = new System.Drawing.Point(14, 552);
            this.TypeLabel.Name = "TypeLabel";
            this.TypeLabel.Size = new System.Drawing.Size(44, 17);
            this.TypeLabel.TabIndex = 7;
            this.TypeLabel.Text = "Type:";
            // 
            // RulesBox
            // 
            this.RulesBox.Location = new System.Drawing.Point(156, 468);
            this.RulesBox.Name = "RulesBox";
            this.RulesBox.Size = new System.Drawing.Size(100, 22);
            this.RulesBox.TabIndex = 8;
            // 
            // ConvertedManaCostBox
            // 
            this.ConvertedManaCostBox.Location = new System.Drawing.Point(156, 496);
            this.ConvertedManaCostBox.Name = "ConvertedManaCostBox";
            this.ConvertedManaCostBox.Size = new System.Drawing.Size(100, 22);
            this.ConvertedManaCostBox.TabIndex = 9;
            // 
            // ColorBox
            // 
            this.ColorBox.Location = new System.Drawing.Point(156, 524);
            this.ColorBox.Name = "ColorBox";
            this.ColorBox.Size = new System.Drawing.Size(100, 22);
            this.ColorBox.TabIndex = 10;
            // 
            // TypeBox
            // 
            this.TypeBox.Location = new System.Drawing.Point(156, 552);
            this.TypeBox.Name = "TypeBox";
            this.TypeBox.Size = new System.Drawing.Size(100, 22);
            this.TypeBox.TabIndex = 11;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(156, 643);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(100, 34);
            this.SearchButton.TabIndex = 12;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoLabel.Location = new System.Drawing.Point(10, 29);
            this.InfoLabel.MaximumSize = new System.Drawing.Size(370, 350);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(249, 17);
            this.InfoLabel.TabIndex = 13;
            this.InfoLabel.Text = "This will show the info of selected card";
            this.InfoLabel.Visible = false;
            // 
            // AddToDeckButton
            // 
            this.AddToDeckButton.Location = new System.Drawing.Point(290, 468);
            this.AddToDeckButton.Name = "AddToDeckButton";
            this.AddToDeckButton.Size = new System.Drawing.Size(134, 56);
            this.AddToDeckButton.TabIndex = 14;
            this.AddToDeckButton.Text = "Add To Deck";
            this.AddToDeckButton.UseVisualStyleBackColor = true;
            this.AddToDeckButton.Click += new System.EventHandler(this.AddToDeckButton_Click);
            // 
            // ShowDeckButton
            // 
            this.ShowDeckButton.Location = new System.Drawing.Point(290, 548);
            this.ShowDeckButton.Name = "ShowDeckButton";
            this.ShowDeckButton.Size = new System.Drawing.Size(134, 54);
            this.ShowDeckButton.TabIndex = 15;
            this.ShowDeckButton.Text = "Show Deck";
            this.ShowDeckButton.UseVisualStyleBackColor = true;
            this.ShowDeckButton.Click += new System.EventHandler(this.ShowDeckButton_Click);
            // 
            // CountLabel
            // 
            this.CountLabel.AutoSize = true;
            this.CountLabel.Location = new System.Drawing.Point(694, 660);
            this.CountLabel.Name = "CountLabel";
            this.CountLabel.Size = new System.Drawing.Size(90, 17);
            this.CountLabel.TabIndex = 16;
            this.CountLabel.Text = "Count In List:";
            // 
            // RulesAloneLabel
            // 
            this.RulesAloneLabel.AutoSize = true;
            this.RulesAloneLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RulesAloneLabel.Location = new System.Drawing.Point(447, 27);
            this.RulesAloneLabel.MaximumSize = new System.Drawing.Size(250, 500);
            this.RulesAloneLabel.Name = "RulesAloneLabel";
            this.RulesAloneLabel.Size = new System.Drawing.Size(225, 34);
            this.RulesAloneLabel.TabIndex = 17;
            this.RulesAloneLabel.Text = "The rules text of selected card will appear here.";
            this.RulesAloneLabel.Visible = false;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(14, 608);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(53, 17);
            this.NameLabel.TabIndex = 18;
            this.NameLabel.Text = "Name: ";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(156, 608);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(100, 22);
            this.NameTextBox.TabIndex = 19;
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(12, 643);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(92, 34);
            this.ClearButton.TabIndex = 20;
            this.ClearButton.Text = "Clear Fields";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(290, 622);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(134, 55);
            this.RemoveButton.TabIndex = 21;
            this.RemoveButton.Text = "Remove From Deck";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Visible = false;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripButton,
            this.SettingButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1048, 27);
            this.toolStrip1.TabIndex = 22;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FileToolStripButton
            // 
            this.FileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.FileToolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveDeckToolStripMenuItem,
            this.loadDeckToolStripMenuItem,
            this.newDeckToolStripMenuItem,
            this.createPrintableToolStripMenuItem});
            this.FileToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("FileToolStripButton.Image")));
            this.FileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileToolStripButton.Name = "FileToolStripButton";
            this.FileToolStripButton.Size = new System.Drawing.Size(48, 24);
            this.FileToolStripButton.Text = "File";
            // 
            // saveDeckToolStripMenuItem
            // 
            this.saveDeckToolStripMenuItem.Name = "saveDeckToolStripMenuItem";
            this.saveDeckToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.saveDeckToolStripMenuItem.Text = "Save Deck";
            this.saveDeckToolStripMenuItem.Click += new System.EventHandler(this.saveDeckToolStripMenuItem_Click);
            // 
            // loadDeckToolStripMenuItem
            // 
            this.loadDeckToolStripMenuItem.Name = "loadDeckToolStripMenuItem";
            this.loadDeckToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.loadDeckToolStripMenuItem.Text = "Load Deck";
            this.loadDeckToolStripMenuItem.Click += new System.EventHandler(this.loadDeckToolStripMenuItem_Click);
            // 
            // newDeckToolStripMenuItem
            // 
            this.newDeckToolStripMenuItem.Name = "newDeckToolStripMenuItem";
            this.newDeckToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.newDeckToolStripMenuItem.Text = "New Deck";
            this.newDeckToolStripMenuItem.Click += new System.EventHandler(this.newDeckToolStripMenuItem_Click);
            // 
            // createPrintableToolStripMenuItem
            // 
            this.createPrintableToolStripMenuItem.Name = "createPrintableToolStripMenuItem";
            this.createPrintableToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.createPrintableToolStripMenuItem.Text = "Print Deck";
            this.createPrintableToolStripMenuItem.Click += new System.EventHandler(this.createPrintableToolStripMenuItem_Click);
            // 
            // SettingButton
            // 
            this.SettingButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.SettingButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateCardsDatabaseToolStripMenuItem});
            this.SettingButton.Image = ((System.Drawing.Image)(resources.GetObject("SettingButton.Image")));
            this.SettingButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SettingButton.Name = "SettingButton";
            this.SettingButton.Size = new System.Drawing.Size(75, 24);
            this.SettingButton.Text = "Settings";
            // 
            // updateCardsDatabaseToolStripMenuItem
            // 
            this.updateCardsDatabaseToolStripMenuItem.Name = "updateCardsDatabaseToolStripMenuItem";
            this.updateCardsDatabaseToolStripMenuItem.Size = new System.Drawing.Size(235, 24);
            this.updateCardsDatabaseToolStripMenuItem.Text = "Update Cards Database";
            this.updateCardsDatabaseToolStripMenuItem.Click += new System.EventHandler(this.updateCardsDatabaseToolStripMenuItem_Click);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "openFileDialog1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(694, 685);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(103, 17);
            this.StatusLabel.TabIndex = 23;
            this.StatusLabel.Text = "Current Status:";
            // 
            // RecentSelectionBox
            // 
            this.RecentSelectionBox.FormattingEnabled = true;
            this.RecentSelectionBox.ItemHeight = 16;
            this.RecentSelectionBox.Location = new System.Drawing.Point(450, 445);
            this.RecentSelectionBox.Name = "RecentSelectionBox";
            this.RecentSelectionBox.Size = new System.Drawing.Size(222, 212);
            this.RecentSelectionBox.TabIndex = 24;
            this.RecentSelectionBox.SelectedIndexChanged += new System.EventHandler(this.RecentSelectionBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(450, 422);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 17);
            this.label2.TabIndex = 25;
            this.label2.Text = "Recent Selections:";
            // 
            // ListStatusLabel
            // 
            this.ListStatusLabel.AutoSize = true;
            this.ListStatusLabel.Location = new System.Drawing.Point(697, 31);
            this.ListStatusLabel.Name = "ListStatusLabel";
            this.ListStatusLabel.Size = new System.Drawing.Size(83, 17);
            this.ListStatusLabel.TabIndex = 26;
            this.ListStatusLabel.Text = "Search List:";
            // 
            // SuperTypeLabel
            // 
            this.SuperTypeLabel.AutoSize = true;
            this.SuperTypeLabel.Location = new System.Drawing.Point(14, 580);
            this.SuperTypeLabel.Name = "SuperTypeLabel";
            this.SuperTypeLabel.Size = new System.Drawing.Size(78, 17);
            this.SuperTypeLabel.TabIndex = 27;
            this.SuperTypeLabel.Text = "Card Type:";
            // 
            // SuperTypeTextBox
            // 
            this.SuperTypeTextBox.Location = new System.Drawing.Point(156, 580);
            this.SuperTypeTextBox.Name = "SuperTypeTextBox";
            this.SuperTypeTextBox.Size = new System.Drawing.Size(100, 22);
            this.SuperTypeTextBox.TabIndex = 28;
            // 
            // DeckBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 711);
            this.Controls.Add(this.SuperTypeTextBox);
            this.Controls.Add(this.SuperTypeLabel);
            this.Controls.Add(this.ListStatusLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RecentSelectionBox);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.RulesAloneLabel);
            this.Controls.Add(this.CountLabel);
            this.Controls.Add(this.ShowDeckButton);
            this.Controls.Add(this.AddToDeckButton);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.TypeBox);
            this.Controls.Add(this.ColorBox);
            this.Controls.Add(this.ConvertedManaCostBox);
            this.Controls.Add(this.RulesBox);
            this.Controls.Add(this.TypeLabel);
            this.Controls.Add(this.ColorLabel);
            this.Controls.Add(this.ConvertedManaCostLabel);
            this.Controls.Add(this.RulesLabel);
            this.Controls.Add(this.CardList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CardImageBox);
            this.Name = "DeckBuilder";
            this.Text = " Magic The Appering ";
            ((System.ComponentModel.ISupportInitialize)(this.CardImageBox)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox CardImageBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox CardList;
        private System.Windows.Forms.Label RulesLabel;
        private System.Windows.Forms.Label ConvertedManaCostLabel;
        private System.Windows.Forms.Label ColorLabel;
        private System.Windows.Forms.Label TypeLabel;
        private System.Windows.Forms.TextBox RulesBox;
        private System.Windows.Forms.TextBox ConvertedManaCostBox;
        private System.Windows.Forms.TextBox ColorBox;
        private System.Windows.Forms.TextBox TypeBox;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button AddToDeckButton;
        private System.Windows.Forms.Button ShowDeckButton;
        private System.Windows.Forms.Label CountLabel;
        private System.Windows.Forms.Label RulesAloneLabel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton FileToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem saveDeckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadDeckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newDeckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createPrintableToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.ListBox RecentSelectionBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label ListStatusLabel;
        private System.Windows.Forms.Label SuperTypeLabel;
        private System.Windows.Forms.TextBox SuperTypeTextBox;
        private System.Windows.Forms.ToolStripDropDownButton SettingButton;
        private System.Windows.Forms.ToolStripMenuItem updateCardsDatabaseToolStripMenuItem;
    }
}

