using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;
using System.Windows.Media;
using System.ComponentModel;
using System.Globalization;

namespace ColumnGuide
{
    #region Adornment Factory
    /// <summary>
    /// Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
    /// that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
    /// </summary>
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class ColumnGuideAdornmentFactory : IWpfTextViewCreationListener
    {
        /// <summary>
        /// Defines the adornment layer for the adornment. This layer is ordered 
        /// below the text in the Z-order
        /// </summary>
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("ColumnGuide")]
        [Order(Before = PredefinedAdornmentLayers.Text)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        /// <summary>
        /// Instantiates a ColumnGuide manager when a textView is created.
        /// </summary>
        /// <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
        public void TextViewCreated(IWpfTextView textView)
        {
            // Always create the adornment, even if there are no guidelines, since we
            // respond to dynamic changes.
            var formatMap = EditorFormatMapService.GetEditorFormatMap(textView);
            new ColumnGuide(textView, TextEditorGuidesSettings, formatMap);
        }

        internal static Brush GetGuidelineBrushFromFontsAndColors(IEditorFormatMap formatMap)
        {
            var resourceDictionary = formatMap.GetProperties(GuidelineColorDefinition.Name);
            if (resourceDictionary.Contains(EditorFormatDefinition.BackgroundBrushId))
            {
                return resourceDictionary[EditorFormatDefinition.BackgroundBrushId] as Brush;
            }

            return null;
        }

        [Import]
        private ITextEditorGuidesSettings TextEditorGuidesSettings { get; set; }

        [Import]
        private IEditorFormatMapService EditorFormatMapService { get; set; }
    }
    #endregion //Adornment Factory
}
