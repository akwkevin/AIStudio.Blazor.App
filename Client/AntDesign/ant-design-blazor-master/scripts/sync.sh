#!/bin/bash

git clone https://github.com/ant-design/ant-design.git
git clone https://github.com/ant-design-blazor/ant-design-blazor.git
cd ant-design
LAST_VERSION=$(git describe --abbrev=0 --tags | sed 's/* //'  )
echo "Last Version of ant-design: ${LAST_VERSION}"
git checkout ${LAST_VERSION}
cd ../ant-design-blazor
BRANCH_NAME="sync-style/${LAST_VERSION}"
if [ $( git branch -a | egrep "remotes/origin/${BRANCH_NAME}" | wc -l) -gt 0 ]; then
  echo "Branch ${BRANCH_NAME} already exists."
  exit 0
fi
echo "Creating a new branch ${BRANCH_NAME} ..."
git checkout -b ${BRANCH_NAME}
cd ../ant-design
echo "Synchronizing the sytle flies to ant-design-blazor..."
find ./components/ -name '*.less' -exec cp --parents -a '{}' '../ant-design-blazor/' ';'
cd ../ant-design-blazor
TOTAL_MODIFIED=$(git status -s | wc -l)
if [ "$TOTAL_MODIFIED" -eq "0" ]; then 
  echo "nothing modified" 
  exit 0
fi
echo "Total modified: ${TOTAL_MODIFIED}"
git config --global user.name 'ElderJames'
git config --global user.email 'shunjiey@hotmail.com'
git add -A
git commit -m "chore: sync ant-design v${LAST_VERSION}"
git push https://ElderJames:$GITHUB_TOKEN@github.com/ElderJames/ant-design-blazor.git ${BRANCH_NAME}
curl -fsSL https://github.com/github/hub/raw/master/script/get | bash -s 2.14.1
cat>PR<<EOF
chore: sync ant-design v${LAST_VERSION}
EOF
bin/hub pull-request -F PR -b ant-design-blazor:master -h ElderJames:${BRANCH_NAME} -a ElderJames
