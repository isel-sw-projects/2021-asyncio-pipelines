import fs from 'fs';
import path from 'path';
import { promisify } from 'util';
import { from } from 'rxjs';
import { map, filter, mergeMap, reduce, concatMap, skip, takeWhile } from 'rxjs/operators';
import { lastValueFrom } from 'rxjs';

const readFile = promisify(fs.readFile);
const readdir = promisify(fs.readdir);

async function* getFilesFromDirectoryGenerator(dir) {
  const dirents = await readdir(dir, { withFileTypes: true });
  for (const dirent of dirents) {
    const filePath = path.resolve(dir, dirent.name);
    if (dirent.isDirectory()) {
      yield* getFilesFromDirectoryGenerator(filePath);
    } else if (dirent.name.endsWith('.txt')) {
      yield filePath;
    }
  }
}

function countWordsInFile(filePath, minLength, maxLength) {
  return from(readFile(filePath, 'utf-8')).pipe(
    map((fileContent) => fileContent.split('\n')),
    concatMap((lines) => from(lines)),
    skip(14),
    takeWhile(line => !line.includes('*** END OF ')),
    filter((line) => line.length > 0),
    concatMap((line) => from(line.split(' '))),
    map(word => word.toLowerCase()),
    filter(word => word !== '' && word.length > minLength && word.length < maxLength),
    reduce((acc, val) => {
      acc[val] = (acc[val] || 0) + 1;
      return acc;
    }, {})
  );
}

async function countWordsInDirectory(directoryPath, minLength=5, maxLength=10) {
  const files = [];
  for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
    files.push(filePath);
  }

  return await lastValueFrom(
    from(files).pipe(
      mergeMap((filePath) => countWordsInFile(filePath, minLength, maxLength)),
      reduce((acc, val) => {
        for (let word in val) {
          acc[word] = (acc[word] || 0) + val[word];
        }
        return acc;
      }, {})
    ));
}

async function benchmark() {
   const folderPath = process.env.TESE_BOOKS_FOLDER_PATH || 'default/path/if/not/set';
  try {
      console.log('Starting pipeline benchmark:');
      console.time('Pipeline Benchmark');
      const wordCounts = await countWordsInDirectory(folderPath);
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


export default benchmark;
