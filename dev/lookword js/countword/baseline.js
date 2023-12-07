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

async function countWordsInDirectory(directoryPath, minLength=5, maxLength=10) {
    const wordCounts = {};
    for await (const filePath of getFilesFromDirectoryGenerator(directoryPath)) {
        const fileWordCounts = await countWordsInFile(filePath, minLength, maxLength);
        for (const [word, count] of Object.entries(fileWordCounts)) {
            wordCounts[word] = (wordCounts[word] || 0) + count;
        }
    }
    return wordCounts;
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

