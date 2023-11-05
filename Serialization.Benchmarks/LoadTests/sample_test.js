import http from 'k6/http';
import { sleep, check, fail } from 'k6';

const API_BASEURL = 'http://127.0.0.1:5020/receiver/serializer';
const serializer = 'FlatBuffers';
const requests = JSON.parse(open('./flatbuffers.json'));

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    //executor: 'constant-vus',
    scenarios: {
        //Serialize: {
        //    exec: 'sendSerialize',
        //    executor: 'ramping-vus',
        //    startTime: '0s',
        //    stages: [
        //        { target: 5, duration: '5s' },
        //        { target: 50, duration: '5s' },
        //        { target: 100, duration: '5s' },
        //        { target: 150, duration: '5s' },
        //        { target: 150, duration: '20s' },
        //        { target: 200, duration: '5s' },
        //        { target: 200, duration: '20s' },
        //        { target: 0, duration: '20s' },
        //    ],
        //},
        Deserialize: {
            executor: 'ramping-vus',
            //startTime: '120s',
            startTime: '0s',
            stages: [
                { target: 5, duration: '5s' },
                { target: 50, duration: '5s' },
                //{ target: 100, duration: '5s' },
                //{ target: 150, duration: '5s' },
                //{ target: 150, duration: '20s' },
                //{ target: 200, duration: '5s' },
                //{ target: 200, duration: '20s' },
                //{ target: 0, duration: '20s' },
                { target: 0, duration: '5s' },
            ],
            exec: 'sendDeserialize'
        },
        //SerializeAndDeserialize: {
        //    executor: 'ramping-vus',
        //    //startTime: '240s',
        //    startTime: '0s',
        //    stages: [
        //        { target: 5, duration: '5s' },
        //        { target: 50, duration: '5s' },
        //        //{ target: 100, duration: '5s' },
        //        //{ target: 150, duration: '5s' },
        //        //{ target: 150, duration: '20s' },
        //        //{ target: 200, duration: '5s' },
        //        //{ target: 200, duration: '20s' },
        //        //{ target: 0, duration: '20s' },
        //        { target: 0, duration: '5s' },
        //    ],
        //    exec: 'sendSerializeAndDeserialize'
        //}
    },
};

export function sendSerialize() {
    const filteredRequests = requests.filter(request => request.Method === 'Serialize');
    const randomIndex = Math.floor(Math.random() * filteredRequests.length);

    const request = filteredRequests[randomIndex];

    const headers = { 'Content-Type': 'application/json' };

    const response = http.post(request.Url, request.Body, { headers: headers });

    console.log(response)

    sleep(.1);
}

export function sendDeserialize() {
    const filteredRequests = requests.filter(request => request.Method === 'Deserialize');
    const randomIndex = Math.floor(Math.random() * filteredRequests.length);

    const request = filteredRequests[randomIndex];

    const data = {
        field: "requestData",
        file: http.file(request.Body)
    }

    const params = { headers: { 'Content-Type': 'application/octet-stream' } };

    const response = http.post(request.Url, data, params);

    console.log(response)

    sleep(.1);
}

export function sendSerializeAndDeserialize() {
    const filteredRequests = requests.filter(request => request.Method === 'SerializeSerializeAndDeserialize');
    const randomIndex = Math.floor(Math.random() * filteredRequests.length);

    const request = filteredRequests[randomIndex];

    const data = {
        field: "requestData",
        file: http.file(request.Body)
    }

    const params = { headers: { 'Content-Type': 'application/octet-stream' } };

    const response = http.post(request.Url, data, params);
    console.log(response)

    sleep(.1);
}

export default () => {
    var response = http.get(`${API_BASEURL}/test`);
    console.log(response)

    sleep(.1);
};

export function teardown(data) {
    const response = http.post(`${API_BASEURL}/monitoring/save-results?serializerType=${serializer}&serializationType=Mixed&numThreads=0&method=Serialize`);
    console.log('TearDown HTTP Request Status Code:', response.status);

    console.log('TearDown: Test has completed.');
}

export function setup() {
    const response = http.get(`${API_BASEURL}/test`);
    console.log('Control Request Status:', response.status);

    const setupResponse = http.post(`${API_BASEURL}/monitoring/start`);

    let setupCheckResult = check(setupResponse, {
        'response code was 200': (res) => res.status == 200,
    });

    if (!setupCheckResult) {
        fail('Setup request failed');
    }
}