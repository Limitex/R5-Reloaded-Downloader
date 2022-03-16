using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_GUI
{
    public class LinkLabelOpener
    {
        public LinkLabelOpener(MainForm form)
        {
            form.OfficialDiscordLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(
                (sender, e) => _LinkClicked(sender, e, form.OfficialDiscordLinkLabel.Text));
            form.OfficialWebsiteLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(
                (sender, e) => _LinkClicked(sender, e, form.OfficialWebsiteLinkLabel.Text));
        }

        private void _LinkClicked(object sender, LinkLabelLinkClickedEventArgs e, string value)
        {
            MainForm.ProcessStart(value);
        }
    }
}
