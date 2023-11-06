import {
    optionsLatency,
    sendDeserialize,
    sendSerialize,
    sendSerializeAndDeserialize,
    setupInternal,
    teardownInternal,
} from "./utils.js";

const API_BASEURL = "http://127.0.0.1:5020/receiver/serializer";
const serializer = "Protobuf";
const requests = JSON.parse(open("./protobuf.json"));
for (const request of requests) {
    if (request.BinaryFilePath != null) request.file = open(request.BinaryFilePath, "b");
}

/*export let options = optionsGeneral;*/

export let options = optionsLatency;

export function sendDeserializeWrapper() {
    sendDeserialize(requests);
}

export function sendSerializeWrapper() {
    sendSerialize(requests);
}

export function sendSerializeAndDeserializeWrapper() {
    sendSerializeAndDeserialize(requests);
}

export function teardown(data) {
    teardownInternal(serializer, API_BASEURL);
}

export function setup() {
    setupInternal(API_BASEURL);
}

export default () => {
    var response = http.get(`${API_BASEURL}/test`);
    console.log(response);

    sleep(0.1);
};
