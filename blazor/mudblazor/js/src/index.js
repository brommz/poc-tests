import moment from 'moment';
import { createWorker } from 'tesseract.js'
import { Buffer } from 'buffer';

export function GetCurrentTime() {
    return moment().format();
}

export async function RunOCR(base64EncodedImage, lang) {
    console.debug('OCR START');

    let imageBuffer = Buffer.from(base64EncodedImage, "base64");
    console.debug('BUFFER IS OK');

    const worker = await createWorker({
        logger: m => console.debug(m)
    });
    console.debug('WORKER CREATED');

    await worker.load();
    await worker.loadLanguage(lang);
    await worker.initialize(lang);

    console.debug("Recognizing...");

    const { data: { text } } = await worker.recognize(imageBuffer);
    console.debug("Recognized text:", text);

    await worker.terminate();
    return text;
}
