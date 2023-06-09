import fs from 'fs';
import path from 'path';
import { promisify } from 'util';
import { from, of } from 'rxjs';
import { map, filter, mergeMap, skip, takeWhile, reduce, toArray, concatMap } from 'rxjs/operators';

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

function findBiggestWordInFile(filePath) {
  return from(readFile(filePath, 'utf-8')).pipe(
    map((fileContent) => fileContent.split('\n')),
    concatMap((lines) => from(lines)),
    skip(14),
    takeWhile((line) => !line.includes('*** END OF ')),
    filter((line) => line.length > 0),
    concatMap((line) => from(line.split(' '))),
    reduce((biggest, curr) => (curr.length > biggest.length ? curr : biggest), '')
  );
}

async function findBiggestWordInDirectory(directoryPath) {
  const files = [];
  for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
    files.push(filePath);
  }

  return await lastValueFrom(
    from(files).pipe(
      mergeMap((filePath) => findBiggestWordInFile(filePath)),
      reduce((biggest, curr) => (curr.length > biggest.length ? curr : biggest), '')
    ))
}

async function benchmark() {
  const folderPath = 'F:/escola/MEIC/TESE/2021-asyncio-pipelines/dev/lookword c#/lookwords/berg/gutenberg';
  try {
    console.log('Starting benchmark:');
    console.time('Benchmark');
    const biggestWord = await findBiggestWordInDirectory(folderPath);
    console.timeEnd('Benchmark');
    console.log('Biggest word found:', biggestWord);
  } catch (error) {
    console.error('Error:', error);
  }
};
benchmark()
export default benchmark