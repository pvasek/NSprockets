using System.Collections.Generic;
using System.Linq;
using System.IO;
using NSprockets.Abstract;

namespace NSprockets
{   
    public class ParserContext: IParserContext
    {
        private readonly Dictionary<string, DirectiveType> _typeMap = new Dictionary<string, DirectiveType>();
        private readonly AssetFile _currentFile;        

        public ParserContext(AssetFile currentFile)
        {
            Directives = new List<Directive>();
            FilteredContent = new StringWriter();
            _currentFile = currentFile;
            _typeMap.Add("require", DirectiveType.Require);
            _typeMap.Add("require_self", DirectiveType.RequireSelf);
            _typeMap.Add("require_tree", DirectiveType.RequireTree);
            _typeMap.Add("require_directory", DirectiveType.RequireDirectory);

        }

        public List<Directive> Directives { get; private set; }
        public StringWriter FilteredContent { get; private set; }

        public void AddDirective(string directiveText)
        {
            var directiveList = directiveText.Trim().ToLower().Split(' ').AsEnumerable();
            var directive = directiveList.First();
            directiveList = directiveList.Skip(1);
            var directiveType = _typeMap[directive];
            
            if (directiveType == DirectiveType.RequireSelf)
            {
                Directives.Add(new Directive(DirectiveType.RequireSelf, _currentFile.Name));
            }
            else
            {
                Directives.AddRange(
                    directiveList
                        .Select(i => new Directive(directiveType, i))); 
            }
        }
    }
}
