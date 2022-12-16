using ColossalFramework.Globalization;
using ColossalFramework.UI;
using System;
using UnityEngine;

namespace BuildingUsage
{
    public class LevelsDetailPanel : UIPanel
    {
        // text
        private const float DisabledTextMultiplier = 0.6f;
        private static readonly Color32 _textColorNormal = new Color32(185, 221, 254, 255);
        private static readonly Color32 _textColorDisabled = new Color32((byte)(_textColorNormal.r * DisabledTextMultiplier), (byte)(_textColorNormal.g * DisabledTextMultiplier), (byte)(_textColorNormal.b * DisabledTextMultiplier), 255);
        private UIFont _textFont;

        // the UI elements for a detail row
        private class DetailRow
        {
            public UILabel Description;
            public UILabel ResidentialCount;
            public UILabel CommercialCount;
            public UILabel OfficesCount;
            public UILabel IndustrialCount;
        }

        // the detail rows
        private DetailRow _heading;
        private DetailRow _level1;
        private DetailRow _level2;
        private DetailRow _level3;
        private DetailRow _level4;
        private DetailRow _level5;
        private DetailRow _total;
        private DetailRow _average;

        // the detail data for each zone and level
        private uint _residentialLevel1;
        private uint _residentialLevel2;
        private uint _residentialLevel3;
        private uint _residentialLevel4;
        private uint _residentialLevel5;

        private uint _commercialLevel1;
        private uint _commercialLevel2;
        private uint _commercialLevel3;

        private uint _officesLevel1;
        private uint _officesLevel2;
        private uint _officesLevel3;

        private uint _industrialLevel1;
        private uint _industrialLevel2;
        private uint _industrialLevel3;

        // miscellaneous for this panel
        private long _previousTicks = 0;
        private bool _initialDataObtained;

        /// <summary>
        /// the bottom position of the bottom-most UI element on this panel
        /// </summary>
        public float BottomPosition { get { return relativePosition.y + size.y; } }

        /// <summary>
        /// Start is called once after the panel is created
        /// set up and populate the panel
        /// </summary>
        public override void Start()
        {
            // do base processing
            base.Start();

            // get the LevelsInfoViewPanel panel (displayed when the user clicks on the Levels info view button)
            LevelsInfoViewPanel levelsPanel = UIView.library.Get<LevelsInfoViewPanel>(typeof(LevelsInfoViewPanel).Name);
            if (levelsPanel == null)
            {
                LogUtil.LogError("Unable to find LevelsInfoViewPanel.");
                return;
            }

            // find the Legend
            UIComponent legend = null;
            foreach (UIComponent comp in levelsPanel.component.components)
            {
                if (comp.name == "Legend")
                {
                    legend = comp;
                    break;
                }
            }
            if (legend == null)
            {
                LogUtil.LogError("Unable to find Legend on LevelsInfoViewPanel.");
                return;
            }

            // move this panel below the legend panel
            relativePosition = new Vector3(legend.relativePosition.x, legend.relativePosition.y + legend.size.y + legend.relativePosition.x);

            // size this panel's width to same as legend panel
            autoSize = false;
            size = new Vector2(legend.size.x, size.y);

            // get text font from the existing label
            UILabel fontTemplate = levelsPanel.Find<UILabel>("ResidentialLevel");
            if (fontTemplate == null)
            {
                LogUtil.LogError("Unable to find ResidentialLevel.");
                return;
            }
            _textFont = fontTemplate.font;

            // create heading row
            if (!CreateDetailRow("Heading", "", 0f, out _heading)) return;

            // adjust heading text size
            const float HeadingTextScale = 0.625f;
            _heading.ResidentialCount.textScale = HeadingTextScale;
            _heading.CommercialCount.textScale  = HeadingTextScale;
            _heading.OfficesCount.textScale     = HeadingTextScale;
            _heading.IndustrialCount.textScale  = HeadingTextScale;

            // adjust heading text
            _heading.ResidentialCount.text = "Residential";
            _heading.CommercialCount.text  = "Commercial";
            _heading.OfficesCount.text     = "Offices";
            _heading.IndustrialCount.text  = "Industrial";

            // set heading tool tips
            _heading.ResidentialCount.tooltip = "Households";
            _heading.CommercialCount.tooltip  = "Workers";
            _heading.OfficesCount.tooltip     = "Workers";
            _heading.IndustrialCount.tooltip  = "Workers";

            // create a line under each heading label
            if (!CreateLineUnderLabel(_heading.ResidentialCount)) return;
            if (!CreateLineUnderLabel(_heading.CommercialCount )) return;
            if (!CreateLineUnderLabel(_heading.OfficesCount    )) return;
            if (!CreateLineUnderLabel(_heading.IndustrialCount )) return;

            // create a detail row for each building level from 1 to 5
            float positionY = _heading.Description.relativePosition.y + _heading.Description.size.y + 15f;
            const float VerticalSpacing = 5f;
            if (!CreateDetailRow("Level1", "Level 1", positionY, out _level1)) return; positionY += _level1.Description.size.y + VerticalSpacing;
            if (!CreateDetailRow("Level2", "Level 2", positionY, out _level2)) return; positionY += _level2.Description.size.y + VerticalSpacing;
            if (!CreateDetailRow("Level3", "Level 3", positionY, out _level3)) return; positionY += _level3.Description.size.y + VerticalSpacing;
            if (!CreateDetailRow("Level4", "Level 4", positionY, out _level4)) return; positionY += _level4.Description.size.y + VerticalSpacing;
            if (!CreateDetailRow("Level5", "Level 5", positionY, out _level5)) return;

            // create lines under level 5
            if (!CreateLineUnderLabel(_level5.Description     )) return;
            if (!CreateLineUnderLabel(_level5.ResidentialCount)) return;
            if (!CreateLineUnderLabel(_level5.CommercialCount )) return;
            if (!CreateLineUnderLabel(_level5.OfficesCount    )) return;
            if (!CreateLineUnderLabel(_level5.IndustrialCount )) return;

            // create detail rows for total and average
            positionY = _level5.Description.relativePosition.y + _level5.Description.size.y + 15f;
            if (!CreateDetailRow("Total",   "Total",   positionY, out _total  )) return; positionY += _total.Description.size.y + VerticalSpacing;
            if (!CreateDetailRow("Average", "Average", positionY, out _average)) return;

            // set tool tips
            _total.Description.tooltip = "Total across all levels";
            _average.Description.tooltip = "Weighted average level";

            // size this panel's height to just hold components
            autoSize = false;
            size = new Vector2(size.x, _average.Description.relativePosition.y + _average.Description.size.y);
        }

        /// <summary>
        /// create a detail row
        /// </summary>
        private bool CreateDetailRow(string namePrefix, string headingText, float positionY, out DetailRow detailRow)
        {
            // create a detail row
            detailRow = new DetailRow();

            // create the description label
            if (!CreateDetailRowLabel(namePrefix + "Description", 70f, 0f, positionY, out detailRow.Description)) return false;
            detailRow.Description.text = headingText;
            detailRow.Description.textAlignment = UIHorizontalAlignment.Left;

            // create a count label for each zone type
            const float Width = 70f;
            const float Spacing = 5f;
            float positionX = size.x - 4f * Width - 3f * Spacing;   // align Industrial at right side of panel
            if (!CreateDetailRowLabel(namePrefix + "Residential", Width, positionX, positionY, out detailRow.ResidentialCount)) return false; positionX += Width + Spacing;
            if (!CreateDetailRowLabel(namePrefix + "Commercial",  Width, positionX, positionY, out detailRow.CommercialCount )) return false; positionX += Width + Spacing;
            if (!CreateDetailRowLabel(namePrefix + "Offices",     Width, positionX, positionY, out detailRow.OfficesCount    )) return false; positionX += Width + Spacing;
            if (!CreateDetailRowLabel(namePrefix + "Industrial",  Width, positionX, positionY, out detailRow.IndustrialCount )) return false;

            // success
            return true;
        }

        /// <summary>
        /// create a detail row label
        /// </summary>
        private bool CreateDetailRowLabel(string namePrefix, float sizeX, float positionX, float positionY, out UILabel label)
        {
            // create the heading label
            label = AddUIComponent<UILabel>();
            if (label == null)
            {
                LogUtil.LogError("Unable to create detail row label on LevelsInfoViewPanel.");
                return false;
            }
            label.name = namePrefix + "Count";
            label.font = _textFont;
            label.text = "";
            label.textAlignment = UIHorizontalAlignment.Right;
            label.verticalAlignment = UIVerticalAlignment.Middle;
            label.textScale = 0.75f;
            label.textColor = _textColorDisabled;
            label.autoSize = false;
            label.size = new Vector2(sizeX, 15f);
            label.relativePosition = new Vector3(positionX, positionY);
            label.isVisible = true;

            // success
            return true;
        }

        /// <summary>
        /// create a line under the label
        /// </summary>
        private bool CreateLineUnderLabel(UILabel label)
        {
            // create the sprite
            UISprite line = AddUIComponent<UISprite>();
            if (line == null)
            {
                LogUtil.LogError("Unable to create line on LevelsInfoViewPanel.");
                return false;
            }
            line.name = label.name + "Line";
            line.autoSize = false;
            line.size = new Vector2(label.size.x, 5f);
            line.relativePosition = new Vector3(label.relativePosition.x, label.relativePosition.y + label.size.y);
            line.spriteName = "ButtonMenuMain";
            line.isVisible = true;

            // success
            return true;
        }

        /// <summary>
        /// get detail data from the district
        /// called right before District.SimulationStep because that is when the m_tempLevel# values are valid and used to compute m_finalLevel#
        /// cannot use the m_finalLevel# values because they are percents
        /// </summary>
        public void GetDetailData(byte districtID)
        {
            // only do district 0 (i.e. entire city)
            if (districtID != 0)
            {
                return;
            }

            // check conditions
            if (!DistrictManager.exists)
            {
                return;
            }

            // get district 0 (i.e. for whole city)
            District districtZero = DistrictManager.instance.m_districts.m_buffer[0];

            // get the detail data for each zone and level
            _residentialLevel1 = districtZero.m_residentialData.m_tempLevel1;
            _residentialLevel2 = districtZero.m_residentialData.m_tempLevel2;
            _residentialLevel3 = districtZero.m_residentialData.m_tempLevel3;
            _residentialLevel4 = districtZero.m_residentialData.m_tempLevel4;
            _residentialLevel5 = districtZero.m_residentialData.m_tempLevel5;

            _commercialLevel1 = districtZero.m_commercialData.m_tempLevel1;
            _commercialLevel2 = districtZero.m_commercialData.m_tempLevel2;
            _commercialLevel3 = districtZero.m_commercialData.m_tempLevel3;

            _officesLevel1 = districtZero.m_officeData.m_tempLevel1;
            _officesLevel2 = districtZero.m_officeData.m_tempLevel2;
            _officesLevel3 = districtZero.m_officeData.m_tempLevel3;

            _industrialLevel1 = districtZero.m_industrialData.m_tempLevel1;
            _industrialLevel2 = districtZero.m_industrialData.m_tempLevel2;
            _industrialLevel3 = districtZero.m_industrialData.m_tempLevel3;

            // initial data has been obtained
            _initialDataObtained = true;
        }

        /// <summary>
        /// update the panel
        /// </summary>
        public void UpdatePanel()
        {
            try
            {
                // check conditions
                if (!InfoManager.exists)
                {
                    return;
                }
                if (!UnlockManager.exists)
                {
                    return;
                }

                // info mode can change, make sure info mode is still BuildingLevel
                if (InfoManager.instance.CurrentMode != InfoManager.InfoMode.BuildingLevel)
                {
                    return;
                }

                // update every 1 second
                long currentTicks = DateTime.Now.Ticks;
                if (currentTicks - _previousTicks >= 1 * TimeSpan.TicksPerSecond)
                {
                    // do residential if unlocked
                    UnlockManager instance = UnlockManager.instance;
                    if (instance.Unlocked(ItemClass.Zone.ResidentialLow) || instance.Unlocked(ItemClass.Zone.ResidentialHigh))
                    {
                        // set text color to normal (never goes back to disabled because zone type never goes back to locked)
                        _heading.ResidentialCount.textColor = _textColorNormal;
                        _level1.ResidentialCount.textColor  = _textColorNormal;
                        _level2.ResidentialCount.textColor  = _textColorNormal;
                        _level3.ResidentialCount.textColor  = _textColorNormal;
                        _level4.ResidentialCount.textColor  = _textColorNormal;
                        _level5.ResidentialCount.textColor  = _textColorNormal;
                        _total.ResidentialCount.textColor   = _textColorNormal;
                        _average.ResidentialCount.textColor = _textColorNormal;

                        // when residential is unlocked, also set description text color to normal
                        _level1.Description.textColor  = _textColorNormal;
                        _level2.Description.textColor  = _textColorNormal;
                        _level3.Description.textColor  = _textColorNormal;
                        _level4.Description.textColor  = _textColorNormal;
                        _level5.Description.textColor  = _textColorNormal;
                        _total.Description.textColor   = _textColorNormal;
                        _average.Description.textColor = _textColorNormal;

                        // continue only if initial data was obtained
                        if (_initialDataObtained)
                        {
                            // compute total and average (average is weighted by level)
                            uint total = _residentialLevel1 + _residentialLevel2 + _residentialLevel3 + _residentialLevel4 + _residentialLevel5;
                            float average = 0f;
                            if (total != 0)
                            {
                                average = (float)(1 * _residentialLevel1 + 2 * _residentialLevel2 + 3 * _residentialLevel3 + 4 * _residentialLevel4 + 5 * _residentialLevel5) / total;
                            }

                            // display level values
                            _level1.ResidentialCount.text = _residentialLevel1.ToString("N0", LocaleManager.cultureInfo);
                            _level2.ResidentialCount.text = _residentialLevel2.ToString("N0", LocaleManager.cultureInfo);
                            _level3.ResidentialCount.text = _residentialLevel3.ToString("N0", LocaleManager.cultureInfo);
                            _level4.ResidentialCount.text = _residentialLevel4.ToString("N0", LocaleManager.cultureInfo);
                            _level5.ResidentialCount.text = _residentialLevel5.ToString("N0", LocaleManager.cultureInfo);
                            _total.ResidentialCount.text = total.ToString("N0", LocaleManager.cultureInfo);
                            _average.ResidentialCount.text = average.ToString("F2", LocaleManager.cultureInfo);
                        }
                    }

                    // do commercial if unlocked
                    if (instance.Unlocked(ItemClass.Zone.CommercialLow) || instance.Unlocked(ItemClass.Zone.CommercialHigh))
                    {
                        // set text color to normal (never goes back to disabled because zone type never goes back to locked)
                        _heading.CommercialCount.textColor = _textColorNormal;
                        _level1.CommercialCount.textColor = _textColorNormal;
                        _level2.CommercialCount.textColor = _textColorNormal;
                        _level3.CommercialCount.textColor = _textColorNormal;
                        _total.CommercialCount.textColor = _textColorNormal;
                        _average.CommercialCount.textColor = _textColorNormal;

                        // continue only if initial data was obtained
                        if (_initialDataObtained)
                        {
                            // compute total and average (average is weighted by level)
                            uint total = _commercialLevel1 + _commercialLevel2 + _commercialLevel3;
                            float average = 0f;
                            if (total != 0)
                            {
                                average = (float)(1 * _commercialLevel1 + 2 * _commercialLevel2 + 3 * _commercialLevel3) / total;
                            }

                            // display level values
                            _level1.CommercialCount.text = _commercialLevel1.ToString("N0", LocaleManager.cultureInfo);
                            _level2.CommercialCount.text = _commercialLevel2.ToString("N0", LocaleManager.cultureInfo);
                            _level3.CommercialCount.text = _commercialLevel3.ToString("N0", LocaleManager.cultureInfo);
                            _total.CommercialCount.text = total.ToString("N0", LocaleManager.cultureInfo);
                            _average.CommercialCount.text = average.ToString("F2", LocaleManager.cultureInfo);
                        }
                    }

                    // do offices if unlocked
                    if (instance.Unlocked(ItemClass.Zone.Office))
                    {
                        // set text color to normal (never goes back to disabled because zone type never goes back to locked)
                        _heading.OfficesCount.textColor = _textColorNormal;
                        _level1.OfficesCount.textColor = _textColorNormal;
                        _level2.OfficesCount.textColor = _textColorNormal;
                        _level3.OfficesCount.textColor = _textColorNormal;
                        _total.OfficesCount.textColor = _textColorNormal;
                        _average.OfficesCount.textColor = _textColorNormal;

                        // continue only if initial data was obtained
                        if (_initialDataObtained)
                        {
                            // compute total and average (average is weighted by level)
                            uint total = _officesLevel1 + _officesLevel2 + _officesLevel3;
                            float average = 0f;
                            if (total != 0)
                            {
                                average = (float)(1 * _officesLevel1 + 2 * _officesLevel2 + 3 * _officesLevel3) / total;
                            }

                            // display level values
                            _level1.OfficesCount.text = _officesLevel1.ToString("N0", LocaleManager.cultureInfo);
                            _level2.OfficesCount.text = _officesLevel2.ToString("N0", LocaleManager.cultureInfo);
                            _level3.OfficesCount.text = _officesLevel3.ToString("N0", LocaleManager.cultureInfo);
                            _total.OfficesCount.text = total.ToString("N0", LocaleManager.cultureInfo);
                            _average.OfficesCount.text = average.ToString("F2", LocaleManager.cultureInfo);
                        }
                    }

                    // do industrial if unlocked
                    if (instance.Unlocked(ItemClass.Zone.Industrial))
                    {
                        // set text color to normal (never goes back to disabled because zone type never goes back to locked)
                        _heading.IndustrialCount.textColor = _textColorNormal;
                        _level1.IndustrialCount.textColor = _textColorNormal;
                        _level2.IndustrialCount.textColor = _textColorNormal;
                        _level3.IndustrialCount.textColor = _textColorNormal;
                        _total.IndustrialCount.textColor = _textColorNormal;
                        _average.IndustrialCount.textColor = _textColorNormal;

                        // continue only if initial data was obtained
                        if (_initialDataObtained)
                        {
                            // compute total and average (average is weighted by level)
                            uint total = _industrialLevel1 + _industrialLevel2 + _industrialLevel3;
                            float average = 0f;
                            if (total != 0)
                            {
                                average = (float)(1 * _industrialLevel1 + 2 * _industrialLevel2 + 3 * _industrialLevel3) / total;
                            }

                            // display level values
                            _level1.IndustrialCount.text = _industrialLevel1.ToString("N0", LocaleManager.cultureInfo);
                            _level2.IndustrialCount.text = _industrialLevel2.ToString("N0", LocaleManager.cultureInfo);
                            _level3.IndustrialCount.text = _industrialLevel3.ToString("N0", LocaleManager.cultureInfo);
                            _total.IndustrialCount.text = total.ToString("N0", LocaleManager.cultureInfo);
                            _average.IndustrialCount.text = average.ToString("F2", LocaleManager.cultureInfo);
                        }
                    }

                    // save ticks for next time
                    _previousTicks = currentTicks;
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }
        }

    }
}
