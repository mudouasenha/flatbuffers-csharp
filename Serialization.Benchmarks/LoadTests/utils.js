﻿import http from 'k6/http';
import { sleep, check, fail } from 'k6';

export const scenariosGeneral = {
    Serialize: {
        exec: 'sendSerializeWrapper',
        executor: 'ramping-vus',
        startTime: '0s',
        stages: [
            { target: 5, duration: '5s' },
            { target: 50, duration: '5s' },
            { target: 100, duration: '5s' },
            { target: 150, duration: '5s' },
            { target: 150, duration: '20s' },
            { target: 200, duration: '5s' },
            { target: 200, duration: '20s' },
            { target: 0, duration: '20s' },
        ],
    },
    Deserialize: {
        executor: 'ramping-vus',
        startTime: '120s',
        //startTime: '0s',
        stages: [
            { target: 5, duration: '5s' },
            { target: 50, duration: '5s' },
            { target: 100, duration: '5s' },
            { target: 150, duration: '5s' },
            { target: 150, duration: '20s' },
            { target: 200, duration: '5s' },
            { target: 200, duration: '20s' },
            { target: 0, duration: '20s' },
            { target: 0, duration: '5s' },
        ],
        exec: 'sendDeserializeWrapper'
    },
    SerializeAndDeserialize: {
        executor: 'ramping-vus',
        startTime: '240s',
        //startTime: '0s',
        stages: [
            { target: 5, duration: '5s' },
            { target: 50, duration: '5s' },
            { target: 100, duration: '5s' },
            { target: 150, duration: '5s' },
            { target: 150, duration: '20s' },
            { target: 200, duration: '5s' },
            { target: 200, duration: '20s' },
            { target: 0, duration: '20s' },
            { target: 0, duration: '5s' },
        ],
        exec: 'sendSerializeAndDeserializeWrapper'
    }
};

export const scenariosTest = {
    SerializeAndDeserialize: {
        executor: 'ramping-vus',
        startTime: '0s',
        stages: [
            { target: 1, duration: '0s' },
            { target: 1, duration: '10s' },
            { target: 0, duration: '5s' },
        ],
        exec: 'sendSerializeAndDeserializeWrapper'
    },
    Serialize: {
        exec: 'sendSerializeWrapper',
        executor: 'ramping-vus',
        startTime: '30s',
        stages: [
            { target: 1, duration: '0s' },
            { target: 1, duration: '10s' },
            { target: 0, duration: '5s' },
        ],
    },
    Deserialize: {
        executor: 'ramping-vus',
        startTime: '60s',
        stages: [
            { target: 1, duration: '0s' },
            { target: 1, duration: '10s' },
            { target: 0, duration: '5s' },
        ],
        exec: 'sendDeserializeWrapper'
    },
};

export const scenariosLatency = {
    Serialize: {
        exec: 'sendSerializeWrapper',
        executor: 'ramping-vus',
        startTime: '15s',
        stages: [
            { target: 5, duration: '1s' },
            { target: 25, duration: '5s' },
            { target: 25, duration: '70s' },
            { target: 50, duration: '5s' },
            { target: 50, duration: '90s' },
            { target: 100, duration: '5s' },
            { target: 100, duration: '100s' },
            { target: 150, duration: '5s' },
            { target: 150, duration: '140s' },
            { target: 200, duration: '5s' },
            { target: 200, duration: '200s' },
            { target: 0, duration: '5s' },
        ],
    },
    Deserialize: {
        executor: 'ramping-vus',
        startTime: '15s',
        //startTime: '0s',
        stages: [
            { target: 5, duration: '1s' },
            { target: 25, duration: '5s' },
            { target: 25, duration: '70s' },
            { target: 50, duration: '5s' },
            { target: 50, duration: '90s' },
            { target: 100, duration: '5s' },
            { target: 100, duration: '100s' },
            { target: 150, duration: '5s' },
            { target: 150, duration: '140s' },
            { target: 200, duration: '5s' },
            { target: 200, duration: '200s' },
            { target: 0, duration: '5s' },
        ],
        exec: 'sendDeserializeWrapper'
    },
    //SerializeAndDeserialize: {
    //    executor: 'ramping-vus',
    //    startTime: '15s',
    //    //startTime: '0s',
    //    stages: [
    //        { target: 5, duration: '1s' },
    //        { target: 50, duration: '5s' },
    //        { target: 50, duration: '90s' },
    //        { target: 100, duration: '5s' },
    //        { target: 100, duration: '90s' },
    //        { target: 200, duration: '5s' },
    //        { target: 200, duration: '90s' },
    //        { target: 300, duration: '5s' },
    //        { target: 300, duration: '140s' },
    //        { target: 400, duration: '5s' },
    //        { target: 400, duration: '200s' },
    //        { target: 0, duration: '5s' },
    //    ],
    //    exec: 'sendSerializeAndDeserializeWrapper'
    //}
};

export const optionsGeneral = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    //executor: 'constant-vus',
    scenarios: scenariosGeneral,
};

export const optionsTest = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    //executor: 'constant-vus',
    scenarios: scenariosTest,
};

export const optionsLatency = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    //executor: 'constant-vus',
    scenarios: scenariosLatency,
};

function decodeBase64ToByteArray(base64String) {
    const rawString = unescape(encodeURIComponent(base64String));
    const byteArray = new Uint8Array(rawString.length);

    for (let i = 0; i < rawString.length; i++) {
        byteArray[i] = rawString.charCodeAt(i);
    }

    return byteArray;
}

function hexStringToByteArray(hexString) {
    // Remove any spaces and convert to uppercase
    hexString = hexString.replace(/\s/g, '').toUpperCase();

    var byteArray = [];
    for (var i = 0; i < hexString.length; i += 2) {
        byteArray.push(parseInt(hexString.substr(i, 2), 16));
    }

    return new Uint8Array(byteArray); // Use Uint8Array for binary data
}

export function teardownInternal(serializer, baseUrl) {
    const response = http.post(`${baseUrl}/monitoring/save-results?serializerType=${serializer}&serializationType=Mixed&numThreads=0&method=Serialize`);
    console.log('TearDown HTTP Request Status Code:', response.status);

    console.log('TearDown: Test has completed.');
}

export function setupInternal(baseUrl) {
    console.log("test")
    const response = http.get(`${baseUrl}/test`);
    console.log('Control Request Status:', response.status);

    const setupResponse = http.post(`${baseUrl}/monitoring/start`);

    let setupCheckResult = check(setupResponse, {
        'response code was 200': (res) => res.status == 200,
    });

    if (!setupCheckResult) {
        fail('Setup request failed');
    }
}

export function sendSerialize(requests) {
    const filteredRequests = requests.filter(request => request.Method === 'Serialize');
    const randomIndex = Math.floor(Math.random() * filteredRequests.length);

    const request = filteredRequests[randomIndex];

    const headers = { 'Content-Type': 'application/json' };

    const response = http.post(request.Url, JSON.stringify(request.Body), { headers: headers });

    //console.log(response)

    sleep(.1);
}

export function sendDeserialize(requests) {
    const filteredRequests = requests.filter(request => request.Method === 'Deserialize');
    const randomIndex = Math.floor(Math.random() * filteredRequests.length);

    const request = filteredRequests[randomIndex];

    //const byteArray = hexStringToByteArray(request.Body);

    const data = {
        field: "requestData",
        file: http.file(request.file)
    }

    const params = { headers: { 'Content-Type': 'application/octet-stream' } };

    const response = http.post(request.Url, data, params);

    //console.log(response)

    sleep(.1);
}

export function sendSerializeAndDeserialize(requests) {
    const filteredRequests = requests.filter(request => request.Method === 'SerializeAndDeserialize');
    const randomIndex = Math.floor(Math.random() * filteredRequests.length);

    const request = filteredRequests[randomIndex];
    //const byteArray = hexStringToByteArray(request.Body);

    const data = {
        field: "requestData",
        file: http.file(request.file)
    }

    const params = { headers: { 'Content-Type': 'application/octet-stream' } };

    const response = http.post(request.Url, data, params);
    //console.log(response)

    sleep(.1);
}