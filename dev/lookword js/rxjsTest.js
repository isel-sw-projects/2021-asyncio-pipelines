const Observable =  require('rxjs')
 
const readline = require('readline')
const fs = require('fs');

const { resolve } = require('path');
const { readdir } = require('fs').promises;

function getFilesFromDirectoryObservable(dir) {
  return new Observable( async ( subscriber ) => {
    const dirents = await readdir(dir, { withFileTypes: true });
    for await (const dirent of dirents) {
        const filename = resolve(dir, dirent.name);
        if (dirent.isDirectory()) {
            getFilesFromDirectoryObservable(filename)
            .subscribe( {
                    next(x) { subscriber.next(x) },
            })
        } else if(dirent.name.endsWith(".txt")) {
            subscriber.next(filename)
        }
        subscriber.complete();
  }})
}


 function getFileWordsGeneratorObservable(filename) {
    return new Observable ( async (subscriber) => {
        const fileStream = fs.createReadStream(filename)
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
            for await (const word of words) {
                subscriber.next(word)
            }
        }
        subscriber.complete();
    })
}

async function JSasyncRxJSTest() {
    const dirname = "C://Users//e351582//OneDrive - EDP//Desktop//TEST"
    const dict = {}
    getFilesFromDirectoryObservable(dirname)
        .subscribe(  {
            next(files) {
                for (var filename of files) {
                    getFileWordsGeneratorObservable(filename)
                        .subscribe( {
                            onNext(words)  { 
                                for (const word in words) {
                                    if(word in dict) {
                                        dict[word] = dict[word] + 1 
                                    } else {
                                        dict[word] = 1
                                    }
                                }
                                for( var element in dict) {
                                    console.log(element + " repetitions: " + dict[element] ) 

                                };
                            } 
                        })
                }
            }
        })
}

JSasyncRxJSTest()