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

async function run() {
  const source = AsyncIterable.range(1, 5);

  const vale = source
    .pipe(
      map(x => x * x)
    );

  const sum = await reduce(vale, {
    seed: 0, // Start with 0 as the accumulator
    callback: async (acc, x, i, signal) => {
      return acc + x;
    }
  });

  console.log(`Sum of squares: ${sum}`);
}

run();
