import fs from 'fs';
import path from 'path';
import { promises as fsPromises } from 'fs';
import { from } from 'ix/iterable';
import { filter, map } from 'ix/iterable/operators';

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
  const fileContent = fs.readFileSync(filePath, 'utf-8');
  const lines = fileContent.split('\n');
  let skipLines = 14;

  return Iterable.from(lines)
    .filter(() => {
      skipLines--;
      return skipLines <= 0;
    })
    .takeWhile(line => !line.includes('*** END OF '))
    .flatMap(line => line.split(' '))
    .filter(word => word.length >= minLength && word.length <= maxLength)
    .reduce((prev, curr) => {
      prev[curr] = (prev[curr] || 0) + 1;
      return prev;
    }, {});
}

async function countWordsInDirectory(directoryPath, minLength, maxLength) {
  return from(getFilesFromDirectoryGenerator(directoryPath))
    .flatMap(filePath => countWordsInFile(filePath, minLength, maxLength))
    .reduce((prev, curr) => {
      for (let word in curr) {
        prev[word] = (prev[word] || 0) + curr[word];
      }
      return prev;
    }, {});
}

async function benchmark() {
  const folderPath =
    'F:/escola/MEIC/TESE/2021-asyncio-pipelines/dev/lookword c#/lookwords/berg/gutenberg';
  try {
    console.log('Starting ix.js benchmark:');
    console.time('ix.js Benchmark');
    await countWordsInDirectory(folderPath, 5, 10);
    console.timeEnd('ix.js Benchmark');
  } catch (error) {
    console.error('Error:', error);
  }
}

benchmark();
export default benchmark;
