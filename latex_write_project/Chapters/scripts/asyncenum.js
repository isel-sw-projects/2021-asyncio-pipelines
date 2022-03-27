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
    let responseSize = 0;
    
    const iterable = streamAsyncIterable(response.body);
    
    for await (const chunk of iterable) {
      responseSize += chunk.length;
    }
    return responseSize;
}  
