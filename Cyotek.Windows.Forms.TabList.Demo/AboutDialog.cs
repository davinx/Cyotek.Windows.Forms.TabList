using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CommonMark;
using Cyotek.Windows.Forms.Demo.Properties;
using TheArtOfDev.HtmlRenderer.WinForms;

namespace Cyotek.Windows.Forms.Demo
{
  // Cyotek TabList
  // Copyright (c) 2012-2013 Cyotek.
  // https://www.cyotek.com
  // https://www.cyotek.com/blog/tag/tablist

  // Licensed under the MIT License. See LICENSE.txt for the full text.

  // If you use this control in your applications, attribution, donations or contributions are welcome.

  internal partial class AboutDialog : BaseForm
  {
    #region Constructors

    public AboutDialog()
    {
      this.InitializeComponent();
    }

    #endregion

    #region Static Methods

    internal static void ShowAboutDialog()
    {
      using (Form dialog = new AboutDialog())
      {
        dialog.ShowDialog();
      }
    }

    #endregion

    #region Properties

    protected TabControl TabControl
    {
      get { return docsTabControl; }
    }

    #endregion

    #region Methods

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      if (!this.DesignMode)
      {
        FileVersionInfo info;
        Assembly assembly;
        string name;

        assembly = typeof(TabList).Assembly;
        info = FileVersionInfo.GetVersionInfo(assembly.Location);
        name = info.ProductName;

        this.Text = $"About {name}";
        nameLabel.Text = name;
        versionLabel.Text = $"Version {info.FileVersion}";
        copyrightLabel.Text = info.LegalCopyright;

        this.AddReadme("CHANGELOG.md");
        this.AddReadme("README.md");
        //this.AddReadme("acknowledgements.md");
        this.AddReadme("LICENSE.txt");

        this.LoadDocumentForTab(docsTabControl.SelectedTab);
      }
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);

      if (docsTabControl != null)
      {
        docsTabControl.SetBounds(docsTabControl.Left, docsTabControl.Top, this.ClientSize.Width - docsTabControl.Left * 2, this.ClientSize.Height - (docsTabControl.Top + footerGroupBox.Height + docsTabControl.Left));
      }
    }

    private void AddReadme(string fileName)
    {
      docsTabControl.TabPages.Add(new TabPage
                                  {
                                    UseVisualStyleBackColor = true,
                                    Padding = new Padding(9),
                                    ToolTipText = this.GetFullReadmePath(fileName),
                                    Text = fileName,
                                    Tag = fileName
                                  });
    }

    private void closeButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void docsTabControl_Selecting(object sender, TabControlCancelEventArgs e)
    {
      this.LoadDocumentForTab(e.TabPage);
    }

    private void footerGroupBox_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.DrawLine(SystemPens.ControlDark, 0, 0, footerGroupBox.Width, 0);
      e.Graphics.DrawLine(SystemPens.ControlLightLight, 0, 1, footerGroupBox.Width, 1);
    }

    private string GetFullReadmePath(string fileName)
    {
      return Path.GetFullPath(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"), fileName));
    }

    private void LoadDocumentForTab(TabPage page)
    {
      if (page != null && page.Controls.Count == 0 && page.Tag != null)
      {
        Control documentView;
        string fullPath;
        string text;

        Cursor.Current = Cursors.WaitCursor;

        Debug.Print("Loading readme: {0}", page.Tag);

        fullPath = this.GetFullReadmePath(page.Tag.ToString());
        text = File.Exists(fullPath) ? File.ReadAllText(fullPath) : $"Cannot find file '{fullPath}'";

        if (text.IndexOf('\n') != -1 && text.IndexOf('\r') == -1)
        {
          text = text.Replace("\n", "\r\n");
        }

        switch (Path.GetExtension(fullPath))
        {
          case ".md":
            documentView = new HtmlPanel
                           {
                             Dock = DockStyle.Fill,
                             BaseStylesheet = Resources.CSS,
                             Text = string.Concat("<html><body>", CommonMarkConverter.Convert(text), "</body></html>") // HACK: HTML panel screws up rendering if a <body> tag isn't present
                           };
            break;
          default:
            documentView = new TextBox
                           {
                             ReadOnly = true,
                             Multiline = true,
                             WordWrap = true,
                             ScrollBars = ScrollBars.Vertical,
                             Dock = DockStyle.Fill,
                             Text = text
                           };
            break;
        }

        page.Controls.Add(documentView);

        Cursor.Current = Cursors.Default;
      }
    }

    private void webLinkLabel_Click(object sender, EventArgs e)
    {
      try
      {
        Process.Start(((Control)sender).Text);
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Unable to start the specified URI.\n\n{ex.Message}", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    #endregion
  }
}
