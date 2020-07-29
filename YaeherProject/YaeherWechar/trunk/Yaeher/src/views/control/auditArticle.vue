<template>
    <div class="audit-article padding-top">
        <mt-header fixed title="文章管理"></mt-header>
        <div class="p-content listAbout">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入关键字查询" />
                    <div @click="searchArticle(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="searchArticle"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="mui-table-view guide-margin">
				<li v-for="(item, index) in articleList" :key="index" class="mui-table-view-cell">
			        <router-link :to="{path: '/article-detail', query: {id: item.id, consultID: item.consultID}}" class="mui-navigate-right articleList">
			        	<div style="width: 60px; height: 60px; padding-left: 0;">
                            <img v-if="item.imageFie !== null" :src="item.imageFie"  alt="" />
                    	    <img v-if="item.imageFie === null" src="../../assets/image/article3.jpg" alt="" />
						</div>
						<div class="articleCase">
                            <span v-if="item.checkState==='Push'" class="hint red-hint">{{item.checkStatus}}</span>
                            <span v-if="item.checkState==='success'" class="hint green-hint">{{item.checkStatus}}</span>
			        		<p class="blod">{{item.paperTiltle}}</p>
			        		<p class="line-text">{{item.paperContent}}</p>
							<p>{{item.checkTime}}</p>
							<i class="mint-cell-allow-right"></i>
			        	</div>
			        </router-link>
			    </li>
			</ul>
        </div>
        <mt-tabbar v-model="selected" fixed>
            <mt-tab-item id="nav1" href="#/index-control">
                <img slot="icon" src="../../assets/image/question-icon.png">
                评价统计
            </mt-tab-item>
            <mt-tab-item id="nav2" href="#/doctor-list-control">
                <img slot="icon" src="../../assets/image/class-icon.png">
                医生查看
            </mt-tab-item>
            <mt-tab-item id="nav3" href="#/audit-article">
                <img slot="icon" src="../../assets/image/class-icon.png">
                文章管理
            </mt-tab-item>
            <mt-tab-item id="nav4" href="#/control-user">
                <img slot="icon" src="../../assets/image/user-icon.png">
                个人中心
            </mt-tab-item>
        </mt-tabbar>
    </div>
</template>

<script>
import { Tabbar, TabItem, Toast } from 'mint-ui';
let moment = require('moment');
export default {
    data () {
        return {
            keyWord: '',
			selected: 'nav3',
			skipCount: 1,
            maxResultCount: 10,
            totalPage: 2,
            articleList: [],
            controlShow: true,
            loading: true,
        }
	},
	methods: {
		searchArticle (flag) {
            this.loading = true
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
			const skipCount = this.skipCount
            const maxResultCount = this.maxResultCount
            const keyWord = this.keyWord
            if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.$refs.inputText.blur()
            this.instance.doctorPaperPageD({
                keyWord,
				skipCount,
				maxResultCount
            })
                .then((response) => {
                    setTimeout(() => {
                        this.loading = false
                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
                        if (flag) this.articleList = []
                        this.articleList = this.articleList.concat(response.data.result.item.items)
                        for (let i = 0; i < this.articleList.length; i++) {
                            this.articleList[i].paperContent = this.articleList[i].paperContent.replace(/<[^>]+>|&/g,"").replace(/nbsp;/g,"")
                            this.articleList[i].checkTime = moment(this.articleList[i].checkTime).format('YYYY-MM-DD HH:mm:ss')
                        }
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        console.log(this.skipCount, this.totalPage)
                    }, 100);
                })
                .catch((error) => {
                    this.loading = true
                }) 
        },
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.searchArticle(true)
            } 
        },
	},
	mounted () {
		this.searchArticle()
	},
	created () {
	}
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";
.audit-article {
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .guide-margin {margin-top: 10px;}
    .articleList {overflow: hidden; display: flex; position: relative;}
    .articleList img {width: 60px; height: 60px; border-radius: 8px;}
    .imgCase {width: 60px; height: 60px;}
    .articleCase {flex: 1; padding-left: 10px;position: relative;}
    .articleCase .hint { position: absolute; right: 0; top: 0;font-size: $font-l;}
    .articleCase p {color: #333; font-size: $font-m;}
    .articleCase .line-text {width: 250px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;}
    .articleCase .blod {font-size: $font-l;}
    .articleCase .blod .red-hint {float: right; font-size: $font-m;}
    .articleCase .blod .green-hint {float: right; font-size: $font-m;}
}

</style>