
var camelCaseTokenizer = function (builder) {

  var pipelineFunction = function (token) {
    var previous = '';
    // split camelCaseString to on each word and combined words
    // e.g. camelCaseTokenizer -> ['camel', 'case', 'camelcase', 'tokenizer', 'camelcasetokenizer']
    var tokenStrings = token.toString().trim().split(/[\s\-]+|(?=[A-Z])/).reduce(function(acc, cur) {
      var current = cur.toLowerCase();
      if (acc.length === 0) {
        previous = current;
        return acc.concat(current);
      }
      previous = previous.concat(current);
      return acc.concat([current, previous]);
    }, []);

    // return token for each string
    // will copy any metadata on input token
    return tokenStrings.map(function(tokenString) {
      return token.clone(function(str) {
        return tokenString;
      })
    });
  }

  lunr.Pipeline.registerFunction(pipelineFunction, 'camelCaseTokenizer')

  builder.pipeline.before(lunr.stemmer, pipelineFunction)
}
var searchModule = function() {
    var documents = [];
    var idMap = [];
    function a(a,b) { 
        documents.push(a);
        idMap.push(b); 
    }

    a(
        {
            id:0,
            title:"INpmContentResolver",
            content:"INpmContentResolver",
            description:'',
            tags:''
        },
        {
            url:'/Cake.Npm.Module/api/Cake.Npm.Module/INpmContentResolver',
            title:"INpmContentResolver",
            description:""
        }
    );
    a(
        {
            id:1,
            title:"ModulesInstallationLocation",
            content:"ModulesInstallationLocation",
            description:'',
            tags:''
        },
        {
            url:'/Cake.Npm.Module/api/Cake.Npm.Module/ModulesInstallationLocation',
            title:"ModulesInstallationLocation",
            description:""
        }
    );
    a(
        {
            id:2,
            title:"Constants",
            content:"Constants",
            description:'',
            tags:''
        },
        {
            url:'/Cake.Npm.Module/api/Cake.Npm.Module/Constants',
            title:"Constants",
            description:""
        }
    );
    a(
        {
            id:3,
            title:"NpmContentResolver",
            content:"NpmContentResolver",
            description:'',
            tags:''
        },
        {
            url:'/Cake.Npm.Module/api/Cake.Npm.Module/NpmContentResolver',
            title:"NpmContentResolver",
            description:""
        }
    );
    a(
        {
            id:4,
            title:"NpmModule",
            content:"NpmModule",
            description:'',
            tags:''
        },
        {
            url:'/Cake.Npm.Module/api/Cake.Npm.Module/NpmModule',
            title:"NpmModule",
            description:""
        }
    );
    a(
        {
            id:5,
            title:"NpmPackageInstaller",
            content:"NpmPackageInstaller",
            description:'',
            tags:''
        },
        {
            url:'/Cake.Npm.Module/api/Cake.Npm.Module/NpmPackageInstaller',
            title:"NpmPackageInstaller",
            description:""
        }
    );
    var idx = lunr(function() {
        this.field('title');
        this.field('content');
        this.field('description');
        this.field('tags');
        this.ref('id');
        this.use(camelCaseTokenizer);

        this.pipeline.remove(lunr.stopWordFilter);
        this.pipeline.remove(lunr.stemmer);
        documents.forEach(function (doc) { this.add(doc) }, this)
    });

    return {
        search: function(q) {
            return idx.search(q).map(function(i) {
                return idMap[i.ref];
            });
        }
    };
}();
