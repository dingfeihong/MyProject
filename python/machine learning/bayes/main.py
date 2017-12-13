from numpy import *

import bayes
listOPosts,listClasses = bayes.loadDataSet()
myVocabList = bayes.createVocabList(listOPosts)
trainMat=[]
for postinDoc in listOPosts:
	trainMat.append(bayes.setOfWords2Vec(myVocabList,postinDoc))
print "train:\t\n",trainMat
