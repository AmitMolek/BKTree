using System;
using System.Collections.Generic;
using System.Text;

namespace BKTree{
    class MatchTree<T>{

		private BKNode root = null;

		public MatchTree() {
		}

		public void add(T term) {
			if (root != null) {
				root.add(term);
			}else {
				root = new BKNode(term);
			}
		}

        public void add(List<T> lst) {
            foreach (T obj in lst) this.add(obj);
        }

		public void clear() {
			root.children.Clear();
			root = null;
		}

		public Dictionary<T, int> query(T searchObject, int thershold) {
			Dictionary<T, int> matches = new Dictionary<T, int>();
			root.query(searchObject, thershold, matches);
			return matches;
		}

		public int find(T term) {
			return root.findBestMatch(term, Int32.MaxValue);
		}

		public T findBestWordMatch(T term) {
			root.findBestMatch(term, Int32.MaxValue);
			return root.getBestTerm();
		}

		public Dictionary<T, int> findBestWordMatchWithDistance(T term) {
			int distance = root.findBestMatch(term, Int32.MaxValue);
			Dictionary<T, int> returnMap = new Dictionary<T, int>();
			returnMap.Add(root.getBestTerm(), distance);
			return returnMap;
		}

		private class Distance {

			public static int calculate(T a, T b) {
				// if the inputs are not strings, than we dont want to calculate the distance between them
				if (a.GetType() != typeof(string) || b.GetType() != typeof(string)) return -1;

				string aStr = a.ToString();
				string bStr = b.ToString();

				if (String.IsNullOrEmpty(aStr)) {
					if (String.IsNullOrEmpty(bStr)) return 0;
					return bStr.Length;
				}
				if (String.IsNullOrEmpty(bStr)) return aStr.Length;

				if (aStr.Length > bStr.Length) {
					string temp = bStr;
					bStr = aStr;
					aStr = temp;
				}

				int m = bStr.Length;
				int n = aStr.Length;
				int[,] distance = new int[2, m + 1];

				for (int j = 1; j <= m; j++) {
					distance[0, j] = j;
				}

				int currentRow = 0;
				for (int i = 1; i <= n; ++i) {
					currentRow = i & 1;
					distance[currentRow, 0] = i;
					int pervRow = currentRow ^ 1;
					for (int j = 1; j <= m; j++) {
						int cost = (bStr[j - 1] == aStr[i - 1] ? 0 : 1);
						distance[currentRow, j] = Math.Min(Math.Min(
							distance[pervRow, j] + 1,
							distance[currentRow, j - 1] + 1),
							distance[pervRow, j - 1]+ cost);
					}
				}
				return distance[currentRow, m];
			}

		}

		private class BKNode {

			T term;
			T bestTerm;
			public Dictionary<int, BKNode> children;

			public BKNode(T term) {
				this.term = term;
				children = new Dictionary<int, BKNode>();
			}

			public void add(T term) {
				int score = Distance.calculate(term, this.term);

				BKNode child = null;
				children.TryGetValue(score, out child);
				if (child != null) {
					child.add(term);
				} else {
					children.Add(score, new BKNode(term));
				}
			}

			public int findBestMatch(T term, int bestDistance) {
				int distanceAtNode = Distance.calculate(term, this.term);

				if (distanceAtNode < bestDistance) {
					bestDistance = distanceAtNode;
					bestTerm = this.term;
				}

				int possibleBest = bestDistance;

				foreach (int score in children.Keys) {
					if (score < distanceAtNode + bestDistance) {
						possibleBest = children.GetValueOrDefault(score).findBestMatch(term, bestDistance);
						if (possibleBest > bestDistance) {
							bestDistance = possibleBest;
						}
					}
				}
				return bestDistance;
			}

			public T getBestTerm() {
				return bestTerm;
			}

			public void query(T term, int thershold, Dictionary<T, int> collected) {
				int distanceAtNode = Distance.calculate(term, this.term);

				if (distanceAtNode == thershold) {
                    if (!collected.ContainsKey(this.term))
                        collected.Add(this.term, distanceAtNode);
					return;
				}

				if (distanceAtNode < thershold) {
                    if (!collected.ContainsKey(this.term))
					    collected.Add(this.term, distanceAtNode);
				}

				for (int score = distanceAtNode - thershold; score <= thershold + distanceAtNode; score++) {
					BKNode child = children.GetValueOrDefault(score);
					if (child != null) {
						child.query(term, thershold, collected);
					}
				}
			}
		}
    }
}
