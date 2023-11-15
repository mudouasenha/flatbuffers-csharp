import { check, fail, sleep } from 'k6';
import http from 'k6/http';

export const scenariosGeneral = {
    Serialize: {
        exec: 'sendSerializeWrapper',
        executor: 'ramping-vus',
        startTime: '0s',
        stages: [
            { target: 200, duration: '5s' },
            { target: 200, duration: '60s' },
            { target: 500, duration: '5s' },
            { target: 500, duration: '60s' },
            { target: 1000, duration: '5s' },
            { target: 1000, duration: '60s' },
            { target: 2000, duration: '10s' },
            { target: 2000, duration: '60s' },
            { target: 3000, duration: '15s' },
            { target: 3000, duration: '60s' },
            { target: 3500, duration: '15s' },
            { target: 3500, duration: '60s' },
            { target: 500, duration: '10s' },
            { target: 0, duration: '10s' },
        ],
    },
    Deserialize: {
        executor: 'ramping-vus',
        startTime: '475s',
        //startTime: '0s',
        stages: [
            { target: 200, duration: '5s' },
            { target: 200, duration: '60s' },
            { target: 500, duration: '5s' },
            { target: 500, duration: '60s' },
            { target: 1000, duration: '5s' },
            { target: 1000, duration: '60s' },
            { target: 2000, duration: '10s' },
            { target: 2000, duration: '60s' },
            { target: 3000, duration: '15s' },
            { target: 3000, duration: '60s' },
            { target: 3500, duration: '15s' },
            { target: 3500, duration: '60s' },
            { target: 500, duration: '10s' },
            { target: 0, duration: '10s' },
        ],
        exec: 'sendDeserializeWrapper'
    }
};

export const scenariosTest = {
    SerializeAndDeserialize: {
        executor: 'ramping-vus',
        startTime: '0s',
        stages: [
            { target: 1, duration: '0s' },
            { target: 1, duration: '1s' },
            { target: 0, duration: '0s' },
        ],
        exec: 'sendSerializeAndDeserializeWrapper'
    },
    Serialize: {
        exec: 'sendSerializeWrapper',
        executor: 'ramping-vus',
        startTime: '0s',
        stages: [
            { target: 1, duration: '0s' },
            { target: 1, duration: '1s' },
            { target: 0, duration: '0s' },
        ],
    },
    Deserialize: {
        executor: 'ramping-vus',
        startTime: '0s',
        stages: [
            { target: 1, duration: '0s' },
            { target: 1, duration: '1s' },
            { target: 0, duration: '0s' },
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

    const response = http.post(request.Url, JSON.parse(request.Body), { headers: headers });
    //console.log(response.status)
    sleep(.1);
}

export function sendDeserialize(requests) {
    const filteredRequests = requests.filter(request => request.Method === 'Deserialize');
    const randomIndex = Math.floor(Math.random() * filteredRequests.length);

    const request = filteredRequests[randomIndex];

    const params = { headers: { 'Content-Type': 'application/octet-stream' } };

    const response = http.post(request.Url, request.file, params);
    //console.log(response.status)

    sleep(.1);
}

export function sendSerializeAndDeserialize(requests) {
    const filteredRequests = requests.filter(request => request.Method === 'SerializeAndDeserialize');
    const randomIndex = Math.floor(Math.random() * filteredRequests.length);

    const request = filteredRequests[randomIndex];

    const params = { headers: { 'Content-Type': 'application/octet-stream' } };

    const response = http.post(request.Url, request.file, params);
    //console.log(response.status)
    sleep(.1);
}

export function generateRequests(serializerType) {
    const numMessages = 1;

    const response = http.post(`http://127.0.0.1:5020/receiver/serializer/generate-requests?serializerType=${serializerType}&numMessages=${numMessages}`);
    console.log(response)
}