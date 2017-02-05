﻿using System;

namespace Cyotek.Windows.Forms.Demo
{
  // Cyotek TabList
  // Copyright (c) 2012-2013 Cyotek.
  // https://www.cyotek.com
  // https://www.cyotek.com/blog/tag/tablist

  // Licensed under the MIT License. See LICENSE.txt for the full text.

  // If you use this control in your applications, attribution, donations or contributions are welcome.

  internal partial class GeneralDemonstrationForm : BaseForm
  {
    #region Constructors

    public GeneralDemonstrationForm()
    {
      this.InitializeComponent();
    }

    #endregion

    #region Methods

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);

      selectionPropertyGrid.SelectedObject = tabList.SelectedPage;
      viewsTabList.SelectedPage = pageTabListPage;
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      AboutDialog.ShowAboutDialog();
    }

    private void addPageButton_Click(object sender, EventArgs e)
    {
      TabListPage page;

      page = tabList.TabListPages.Add("test");
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void firstButton_Click(object sender, EventArgs e)
    {
      this.SelectTab(0);
    }

    private void lastButton_Click(object sender, EventArgs e)
    {
      this.SelectTab(tabList.TabListPageCount - 1);
    }

    private void nextButton_Click(object sender, EventArgs e)
    {
      this.SelectTab(tabList.SelectedIndex + 1);
    }

    private void previousButton_Click(object sender, EventArgs e)
    {
      this.SelectTab(tabList.SelectedIndex - 1);
    }

    private void removePageButton_Click(object sender, EventArgs e)
    {
      TabListPage page;

      page = tabList.SelectedPage;

      if (page != null)
        tabList.TabListPages.Remove(page);
    }

    private void SelectTab(int index)
    {
      if (index < 0)
        index = 0;
      else if (index > tabList.TabListPageCount - 1)
        index = tabList.TabListPageCount - 1;

      tabList.SelectedIndex = index;
    }

    private void tabList_SelectedIndexChanged(object sender, EventArgs e)
    {
      selectionPropertyGrid.SelectedObject = tabList.SelectedPage;
    }

    #endregion
  }
}
