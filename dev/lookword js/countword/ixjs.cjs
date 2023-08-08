const fs = require('fs');
const path = require('path');
const fsPromises = fs.promises;
const { AsyncIterable } = require('ix');
const { map, flatMap, filter, skip, takeWhile } = require('ix/asynciterable/operators');
const { reduce } = require('ix/asynciterable');


async function* getFilesFromDirectoryGenerator(dir) {
  const dirents = await fsPromises.readdir(dir, { withFileTypes: true });
  for (const dirent of dirents) {
    const filePath = path.resolve(dir, dirent.name);
    if (dirent.isDirectory()) {
      yield* getFilesFromDirectoryGenerator(filePath);
    } else if (dirent.isFile() && dirent.name.endsWith('.txt')) {
      yield filePath;
    }
  }
}

function countWordsInFile(filePath, minLength, maxLength) {
  console.log(filePath)
  const vale = AsyncIterable.from(fsPromises.readFile(filePath, 'utf-8'))
  .pipe(
      flatMap((fileContent) => fileContent.split('\n')),
    skip(14),
      takeWhile((line) => !(line.includes('*** END OF '))),
      flatMap((line) => line.split(' ')),
      map(word => word.toLowerCase()),
      filter(word => word !== '' && word.length >= minLength && word.length <= maxLength)
    )

    return reduce(vale, {
      seed: {}, // Start with an empty object as the accumulator
      callback: async (acc, word, i, signal) => {
        
        acc[word] = (acc[word] || 0) + 1;
          
        
        return acc;
      }
    });
  }

async function countWordsInDirectory(directoryPath, minLength, maxLength) {
  const vale =  AsyncIterable.from(getFilesFromDirectoryGenerator(directoryPath))
    .pipe(
      flatMap(async filePath => await countWordsInFile(filePath, minLength, maxLength)))
      
      return reduce(vale, {
        seed: {}, // Start with an empty object as the accumulator
        callback: async (acc, val, i, signal) => {
          for (let word in val) {
              acc[word] = (acc[word] || 0) + val[word];
              
              
          }
          return acc;
        }
      });
}

async function benchmark() {
  const folderPath = 'F:/escola/MEIC/TESE/2021-asyncio-pipelines/books/gutenberg/test';
  try {
    console.log('Starting pipeline benchmark:');
    console.time('Pipeline Benchmark');
    const wordCounts = await countWordsInDirectory(folderPath, 4, 9);
    console.timeEnd('Pipeline Benchmark');

    // Find the most recurring word
    let mostRecurringWord = '';
    let maxCount = 0;
    for (const [word, count] of Object.entries(wordCounts)) {
      if (count > maxCount) {
        mostRecurringWord = word;
        maxCount = count;
      }
    }

    console.log(`Most recurring word: ${mostRecurringWord}, Number of occurrences: ${maxCount}`);
  } catch (error) {
    console.error('Error:', error);
  }
}

benchmark();
