import { b64decode } from "k6/encoding";
import http from "k6/http";
import {
    optionsGeneral,
    sendDeserialize,
    sendSerialize,
    sendSerializeAndDeserialize,
    setupInternal,
    teardownInternal
} from "./utils.js";

const API_BASEURL = "http://127.0.0.1:5020/receiver/serializer";
const serializer = "FlatBuffers";
const requests = JSON.parse(open("./flatbuffers.json"));
requests.forEach(
    (request) => request.ContentType == "application/octet-stream" && (request.file = new Uint8Array(b64decode(request.Body)))
);

export let options = optionsGeneral;

// export let options = optionsLatency;

//export let options = optionsTest;

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
    //requests = ;
    setupInternal(API_BASEURL);
}

export default () => {
    var response = http.get(`${API_BASEURL}/test`);
    console.log(response);

    sleep(0.1);
};
