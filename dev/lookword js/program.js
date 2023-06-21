import baseline_benchmark from './findword/baseline_js.js';
import ixJsStrategy_benchmark from './findword/ixJsStrategy.js';
import rxStrategy_benchmark from './findword/rxStrategy.js';
import highlandStrategy_benchmark from './findword/highlandStrategy.js';

// Array to store the benchmark functions and their names
const benchmarks = [
  { name: 'baseline_js', benchmark: baseline_benchmark },
  { name: 'ixJsStrategy', benchmark: ixJsStrategy_benchmark },
  { name: 'rxStrategy', benchmark: rxStrategy_benchmark },
  { name: 'highlandStrategy', benchmark: highlandStrategy_benchmark },
];

// Function to run benchmark functions multiple times and calculate the average duration
async function runBenchmark(benchmarkObj, runs) {
  const durations = [];

  // Temporarily override console functions
  const originalConsoleTime = console.time;
  const originalConsoleTimeEnd = console.timeEnd;
  const originalConsoleLog = console.log;
  const originalConsoleInfo = console.info;
  const originalConsoleWarn = console.warn;
  const originalConsoleError = console.error;
  
  console.time = () => {};
  console.timeEnd = () => {};
  console.log = () => {};
  console.info = () => {};
  console.warn = () => {};
  console.error = () => {};

  for (let i = 0; i < runs; i++) {
    const start = process.hrtime.bigint();
    await benchmarkObj.benchmark();
    const end = process.hrtime.bigint();

    durations.push(Number(end - start) / 1e6);
  }

  // Restore original console functions
  console.time = originalConsoleTime;
  console.timeEnd = originalConsoleTimeEnd;
  console.log = originalConsoleLog;
  console.info = originalConsoleInfo;
  console.warn = originalConsoleWarn;
  console.error = originalConsoleError;

  const avgDuration = durations.reduce((sum, duration) => sum + duration, 0) / runs;
  return { name: benchmarkObj.name, avgDuration };
}

async function runAllBenchmarks() {
  for (const benchmarkObj of benchmarks) {
    const { name, avgDuration } = await runBenchmark(benchmarkObj, 4);
    console.log(`Average duration for ${name}: ${avgDuration.toFixed(2)} ms`);
  }
}

runAllBenchmarks();
