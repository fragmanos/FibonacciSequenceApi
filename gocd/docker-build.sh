#!/bin/sh

set -eu

# Don't tag with ECR info to prevent upload if scan fails
docker build -t tddPlayground/tddplayground:${GO_REVISION_SOURCE} .
mkdir scan-results
export AQUASEC_SCAN_PASSWORD=$(cat /home/go/aquasec-scanners/build_prod_password)
docker run -v /var/run/docker.sock:/var/run/docker.sock -v $(pwd)/scan-results:/tmp 230575338114.dkr.ecr.us-east-1.amazonaws.com/aquasec/scanner-cli:3.5 scan -H http://aqua-web.aquasec35:8080 -U ${AQUASEC_SCAN_USERNAME} -P ${AQUASEC_SCAN_PASSWORD} --jsonfile /tmp/scan.json --htmlfile /tmp/scan.html --local tddPlayground/tddplayground:${GO_REVISION_SOURCE}
docker tag tddPlayground/tddplayground:${GO_REVISION_SOURCE} 230575338114.dkr.ecr.us-east-1.amazonaws.com/tddPlayground/tddplayground:${GO_REVISION_SOURCE}

FILENAME="./scan-results/scan.json"

# add decision gate for AquaSec report
if [ ! -f ${FILENAME} ]; then
  echo "AQUASEC report ${FILENAME} not found"
  exit 1
fi

if [ -z ${FILENAME} ]; then
  echo "Empty Aquasec output received"
  exit 2
fi

jq . ${FILENAME} >/dev/null
if [ "$?" -ne 0 ]; then
  echo "Malformed Aquasec report received"
  exit 3
fi

HIGH_PRIO_VULN_COUNT=$(jq '.vulnerability_summary.high' ${FILENAME})
MEDIUM_PRIO_VULN_COUNT=$(jq '.vulnerability_summary.medium' ${FILENAME})
LOW_PRIO_VULN_COUNT=$(jq '.vulnerability_summary.low' ${FILENAME})

if [ "${HIGH_PRIO_VULN_COUNT}" -gt 0 ] || [ "${MEDIUM_PRIO_VULN_COUNT}" -gt 0 ] || [ "${LOW_PRIO_VULN_COUNT}" -gt 0 ]; then
  echo "Vulnerabilities identified: please ensure no high, medium or low vulnerabilities remain"
  exit 4
fi
