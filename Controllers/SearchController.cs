using GRESearch.Helper;
using GRESearch.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GRESearch.Controllers
{
  [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
  public class SearchController : ApiController
  {
    private OxfordClientHelper _client = new OxfordClientHelper();
    private string app_id = "42c6327b";
    private string app_key = "41e56a2eb684f21c9dc4da8d52111c5d";
    private string oxfordUrl = "https://od-api.oxforddictionaries.com:443/api/v1/entries/en/";

    public OxfordResponseDTO Get(string word = null)
    {
      string defination = _client.SearchWord(oxfordUrl + word, app_id, app_key);
      var SearchResult = JsonConvert.DeserializeObject<SearchResult>(defination);
      var OxfordResponseDTO = new OxfordResponseDTO();
      string pronunciationStr = string.Empty;
      string definationStr = string.Empty;
      foreach (var results in SearchResult.Results)
      {
        foreach (var entries in results.LexicalEntries)
        {
          if (entries.LexicalCategory.ToLower() == "noun")
          {
            foreach (var proun in entries.Pronunciations)
            {
              pronunciationStr = "AudioFile = ";
              pronunciationStr += proun.AudioFile + ", ";

              pronunciationStr += "Dialects = ";
              pronunciationStr += proun.Dialects[0] + ", ";

              pronunciationStr += "Phonetic Notation = ";
              pronunciationStr += proun.PhoneticNotation + ", ";

              pronunciationStr += "Phonetic Spelling = ";
              pronunciationStr += proun.PhoneticSpelling;
            }

            foreach (var entry in entries.Entries)
            {
              foreach (var sense in entry.Senses)
              {
                if (sense.Definitions != null)
                {
                  definationStr = sense.Definitions[0];
                }
              }
            }

          }
        }
      }

      OxfordResponseDTO.Pronunciation = pronunciationStr;
      OxfordResponseDTO.Definition = definationStr;

      return OxfordResponseDTO;
    }


    public void Post([FromBody]string value)
    {
    }

    public void Put(int id, [FromBody]string value)
    {
    }

    public void Delete(int id)
    {
    }
  }
}
