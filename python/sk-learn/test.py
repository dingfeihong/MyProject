import numpy as np
import urllib.request
from sklearn import preprocessing
from sklearn import metrics
from sklearn.ensemble import ExtraTreesClassifier
from sklearn.linear_model import LogisticRegression
# url with dataset
url = "http://archive.ics.uci.edu/ml/machine-learning-databases/pima-indians-diabetes/pima-indians-diabetes.data"
# download the file
raw_data =  urllib.request.urlopen(url)
# load the CSV file as a numpy matrix
dataset = np.loadtxt(raw_data, delimiter=",")


# separate the data from the target attributes
X = dataset[:,0:7]
y = dataset[:,8]

# normalize the data attributes
normalized_X = preprocessing.normalize(X)
model = LogisticRegression()
model.fit(normalized_X,y)

predicted_y = model.predict(normalized_X)
res=metrics.accuracy_score(y,predicted_y)
print(res)
