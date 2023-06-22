import fs from 'fs';
import path from 'path';
import { promises as fsPromises } from 'fs';

async function* getFilesFromDirectoryGenerator(dir) {
    const dirents = await fsPromises.readdir(dir, { withFileTypes: true });
    for (const dirent of dirents) {
        const filePath = path.resolve(dir, dirent.name);
        if (dirent.isDirectory()) {
            yield* getFilesFromDirectoryGenerator(filePath);
        } else if (dirent.name.endsWith('.txt')) {
            yield filePath;
        }
    }
}

async function countWordsInFile(filePath, minLength, maxLength) {
    const fileContent = await fsPromises.readFile(filePath, 'utf-8');
    const lines = fileContent.split('\n');

    let ignoreLine = false;
    let words = {};
    for (let i = 0; i < lines.length; i++) {
        if (i < 14) continue;

        if (lines[i].includes('*** END OF ')) {
            ignoreLine = true;
        }

        if (ignoreLine) {
            continue;
        }

        const wordsInLine = lines[i].split(' ');
        for (let word of wordsInLine) {
            if (word.length > minLength && word.length < maxLength) {
                word = word.toLowerCase();
                words[word] = (words[word] || 0) + 1;
            }
        }
    }
    return words;
}

async function countWordsInDirectory(directoryPath, minLength, maxLength) {
    const files = [];
    for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
        files.push(filePath);
    }

    const wordCounts = await Promise.all(files.map(file => countWordsInFile(file, minLength, maxLength)));
    return wordCounts.reduce((total, currentCount) => {
        for (const [word, count] of Object.entries(currentCount)) {
            total[word] = (total[word] || 0) + count;
        }
        return total;
    }, {});
}


async function benchmark() {
    const folderPath = 'F:/escola/MEIC/TESE/2021-asyncio-pipelines/dev/lookword c#/lookwords/berg/gutenberg';
    try {
        console.log('Starting baseline benchmark:');
        console.time('Baseline Benchmark');
        const wordCounts = await countWordsInDirectory(folderPath,5,10);
        console.timeEnd('Baseline Benchmark');
    } catch (error) {
        console.error('Error:', error);
    }
};

export default benchmark;

