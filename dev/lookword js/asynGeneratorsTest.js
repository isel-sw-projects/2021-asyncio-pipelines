
const readline = require('readline')
const fs = require('fs');

const { resolve } = require('path');
const { readdir } = require('fs').promises;

async function* getFilesFromDirectoryGenerator(dir) {
  const dirents = await readdir(dir, { withFileTypes: true });
  for (const dirent of dirents) {
    const res = resolve(dir, dirent.name);
    if (dirent.isDirectory()) {
      yield* getFilesFromDirectoryGenerator(res);
    } else if(dirent.name.endsWith(".txt")) {
      yield res;
    }
  }
}


async function* getFileWordsGenerator(filename) {
  
  const fileStream = fs.createReadStream(filename);
  const rl = readline.createInterface({
    input: fileStream,
    crlfDelay: Infinity
  });
  // Note: we use the crlfDelay option to recognize all instances of CR LF
  // ('\r\n') in input.txt as a single line break.

 for await (const line of rl) {
      const words = line.match(/\b[\w']+\b/g)
      if(words == null) {
        continue
      }
      for await(const word of words) {
        yield word
    }
  }
}

async function JSasyncGeneratorTest() {
    const dirname = "C://Users//e351582//OneDrive - EDP//Desktop//TEST"
    const dict = {}
    const files = await getFilesFromDirectoryGenerator(dirname)
    for await(var filename of files) {
      const words = await getFileWordsGenerator(filename)

    for await(const word of words) {
         if(word in dict) {
           dict[word] = dict[word] + 1 
         } else {
           dict[word] = 1
         }
      }
    }

  for( var element in dict) {
      console.log(element + " repetitions: " + dict[element] ) 
    };
     
}

JSasyncGeneratorTest()