async function* streamAsyncIterable(stream) {
    const reader = stream.getReader();
    try {
      while (true) {
        const { done, value } = await reader.read();
        if (done) {
          return;
        }
        yield value;
      }
    } finally {
      reader.releaseLock();
    }
  }

async function getResponseSize(url) {
    const response = await fetch(url);
    // Will hold the size of the response, in bytes.
    let responseSize = 0;
    
    const iterable = streamAsyncIterable(response.body);
    
    for await (const chunk of iterable) {
      // Incrementing the total response length.
      responseSize += chunk.length;
    }
    return responseSize;
}  
