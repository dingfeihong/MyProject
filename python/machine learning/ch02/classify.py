def classify0(inX,dataSet,labels,k):
	dataSetSize = dataSet.shape[0]
	diffMat = tile(inX,(dataSetSize,1))-dataSet
	sqDiffMat = diffMat**2
	sqDistance = sqDiffMat.sum(axis=1)
	distance = sqDistance**0.5
	sortIndex = distance.argsort()
	classCount={}
	for i in range(k):
		voteLabel = labels[sortIndex[i]]
		classCount[voteLabel] = classCount.get(voteLabel,0)+1
	sortedClassCount = sorted(classCount.iteritems(),key=operator.itemgetter(1),reverse=True)
	return sortedClassCount[0][0]