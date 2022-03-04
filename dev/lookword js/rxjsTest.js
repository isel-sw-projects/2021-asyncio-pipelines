import { Observable } from 'rxjs'
import readline from 'readline'
import * as fs from 'fs'
const fspromises = fs.promises

import  {resolve}  from 'path'

function getFilesFromDirectoryObservable(dir) {
    return new Observable( async ( subscriber ) => {
        const dirents = await fs.promises.readdir(dir, { withFileTypes: true });
        for await (const dirent of dirents) {
            const filePath = resolve(dir, dirent.name);
            if (dirent.isDirectory()) {
                getFilesFromDirectoryObservable(filePath)
                .subscribe( {
                        next(fileP) { 
                            subscriber.next(fileP) 
                        },
                })
            } else if(dirent.name.endsWith(".txt")) {
                subscriber.next(filePath)
            }
    }
    })
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
    })
}


 function JSasyncRxJSTest() {
    const dirname = "C://Users//Matrix//Desktop//ola"
    const dict = {}
    return getFilesFromDirectoryObservable(dirname)
    .subscribe(  {
        next(filename) {
            getFileWordsGeneratorObservable(filename)
                .subscribe( {
                    next(word)  { 
                        if(word in dict) {
                            dict[word] = dict[word] + 1 
                        } else {
                            dict[word] = 1
                        }
                }
            })
        }, 
        complete() {
            for( var element in dict) {
                console.log(element + " repetitions: " + dict[element] ) 
            };
        }
    })
}



const obs = JSasyncRxJSTest()

setTimeout(() => {
    
    obs.complete()
}, 20000);